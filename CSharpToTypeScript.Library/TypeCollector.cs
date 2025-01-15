using System.Reflection;
using CSharpToTypeScript.Library.Attributes;
using CSharpToTypeScript.Library.Models;
using CSharpToTypeScript.Library.Models.Properties;

namespace CSharpToTypeScript.Library;

public class TypeCollector(Assembly[] assemblies)
{
    private readonly ConcurrentDictionary<Type, TypeScriptType> _visited = new();

    public Task<ConcurrentDictionary<Type, TypeScriptType>> Collect()
    {
        var typesWithAttribute = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            // IsPublic does not cover nested types
            .Where(type => !type.IsNotPublic && Attribute.IsDefined(type, typeof(TsGenerateAttribute)))
            .ToHashSet();

        foreach (var type in typesWithAttribute)
        {
            CollectReferencedTypes(type);
        }

        return Task.FromResult(_visited);
    }

    private CollectedTypeResult? CollectReferencedTypes(Type type)
    {
        var typeAlreadyVisited = _visited.ContainsKey(type);

        if (typeAlreadyVisited)
        {
            return new(_visited[type], false);
        }

        var isDictionary = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        var isArray = !isDictionary && IsArrayOrEnumerable(type);
        var typeToResolve = isArray ? GetArrayElementType(type) : type;

        if (typeof(Delegate).IsAssignableFrom(type))
        {
            return null;
        }

        if (typeToResolve == typeof(IEquatable<>))
        {
            return null;
        }

        if (typeToResolve is { IsEnum: false, IsValueType: true } || typeToResolve == typeof(string))
        {
            return new(TypeScriptSystemType.Create(typeToResolve), true, isArray);
        }

        if (typeToResolve.IsEnum)
        {
            var tsEnum = new TypeScriptEnum(typeToResolve.Name, Enum.GetNames(typeToResolve).ToList());

            visited.Add(typeToResolve, tsEnum);

            return new(tsEnum, false);
        }

        if (typeToResolve.IsInterface || typeToResolve.IsClass)
        {
            return new(ResolveInterface(typeToResolve), isArray);
        }

        throw new ArgumentException("Type is not supported", nameof(type));
    }

    private TypeScriptInterface ResolveInterface(Type type)
    {
        var typeScriptType = new TypeScriptInterface(type.IsGenericType ? type.Name.Split('`')[0] : type.Name)
        {
            IsOpenGenericType = type.IsGenericTypeDefinition,
            IsGenericParameter = type.IsGenericParameter,
        };

        if (type.IsGenericType)
        {
            var openType = type.GetGenericTypeDefinition();

            if (openType != type)
            {
                CollectReferencedTypes(openType);
            }

            typeScriptType.GenericArguments.AddRange(CollectGenericArguments(type));
        }

        var properties = type.GetProperties().Where(p => p.DeclaringType == type && !IsIgnored(p)).ToList();
        var fields = type.GetFields().Where(f => f.DeclaringType == type && !IsIgnored(f)).ToList();

        var dataMembers = properties.Cast<MemberInfo>().Concat(fields);

        foreach (var dataMember in dataMembers)
        {
            var memberType = dataMember switch
            {
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                FieldInfo fieldInfo => fieldInfo.FieldType,
                _ => throw new InvalidOperationException("Member is not a property or field")
            };

            var isNullable = Nullable.GetUnderlyingType(memberType) is not null || IsMarkedAsNullable(dataMember);
            var isDictionary = memberType.IsGenericType &&
                               memberType.GetGenericTypeDefinition() == typeof(Dictionary<,>);

            if (isDictionary)
            {
                var dictionaryProperty = ResolveDictionaryProperty(memberType, dataMember, isNullable);

                typeScriptType.Properties.Add(dictionaryProperty);

                continue;
            }

            if (CollectReferencedTypes(memberType) is not { } resolvedType) continue;

            if (resolvedType.IsArrayElementType)
            {
                typeScriptType.Properties.Add(new TypeScriptArrayProperty(dataMember.Name,
                    resolvedType.TypeScriptType, isNullable));
            }
            else
            {
                typeScriptType.Properties.Add(new(dataMember.Name, resolvedType.TypeScriptType, isNullable));
            }
        }

        if (type.BaseType != null && !IsSystemType(type.BaseType))
        {
            typeScriptType.BaseType = CollectReferencedTypes(type.BaseType)?.TypeScriptType;
        }

        foreach (var implementedInterface in type.GetInterfaces().Where(implInterface => !IsSystemType(implInterface)))
        {
            if (CollectReferencedTypes(implementedInterface) is { IsSystemType: false } resolvedType)
            {
                typeScriptType.ImplementedInterfaces.Add(resolvedType.TypeScriptType);
            }
        }

        visited.TryAdd(type, typeScriptType);

        return typeScriptType;
    }

    private TypeScriptDictionaryProperty ResolveDictionaryProperty(Type memberType, MemberInfo dataMember,
        bool isNullable)
    {
        var keyType = memberType.GetGenericArguments()[0];

        if (!IsValidDictionaryKeyType(keyType))
        {
            throw new InvalidOperationException("Dictionary key type is not supported");
        }

        var collectedKeyType = CollectReferencedTypes(keyType);
        var collectedValueType = CollectReferencedTypes(memberType.GetGenericArguments()[1]);

        if (collectedKeyType is null || collectedValueType is null)
        {
            throw new InvalidOperationException("Dictionary key or value type is not supported");
        }

        return new(dataMember.Name, collectedKeyType.TypeScriptType, collectedValueType.TypeScriptType, isNullable);
    }

    private static bool IsValidDictionaryKeyType(Type keyType) =>
        keyType == typeof(string) || TypeScriptSystemType.NumberTypeMap.ContainsKey(keyType);

    private List<TypeScriptType> CollectGenericArguments(Type type)
    {
        var genericArguments = new List<TypeScriptType>();

        foreach (var genericTypeArgument in type.GetGenericArguments())
        {
            if (genericTypeArgument.IsGenericTypeParameter)
            {
                genericArguments.Add(new TypeScriptGenericParameter(genericTypeArgument.Name));
            }
            else if (CollectReferencedTypes(genericTypeArgument) is { } resolvedType)
            {
                genericArguments.Add(resolvedType.TypeScriptType);
            }
        }

        return genericArguments;
    }

    private static bool IsMarkedAsNullable(MemberInfo memberInfo)
    {
        var nullabilityContext = new NullabilityInfoContext();

        var nullabilityInfo = memberInfo switch
        {
            PropertyInfo propertyInfo => nullabilityContext.Create(propertyInfo),
            FieldInfo fieldInfo => nullabilityContext.Create(fieldInfo),
            _ => throw new InvalidOperationException("Member is not property or field")
        };

        return nullabilityInfo.WriteState is NullabilityState.Nullable;
    }

    private static bool IsSystemType(Type type) => type.Namespace?.StartsWith("System") == true;

    private static Type GetArrayElementType(Type type)
    {
        // If type implements IEnumerable<> return the generic type argument
        if (type.IsGenericType && IsArrayOrEnumerable(type))
        {
            return type.GetGenericArguments()[0];
        }

        // If type is regular array, return the element type
        return type.GetElementType() ?? throw new ArgumentException("Type is not an array", nameof(type));
    }

    private static bool IsArrayOrEnumerable(Type type)
    {
        if (type == typeof(string))
        {
            return false;
        }

        if (type.IsArray)
        {
            return true;
        }

        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
               type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }

    private static bool IsIgnored(MemberInfo memberInfo) =>
        memberInfo.GetCustomAttributes(typeof(TsIgnoreAttribute), false).Length != 0;
}