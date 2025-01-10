using System.Reflection;
using CSharpToTypeScript.Library.Attributes;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library;

public class TypeCollector(Assembly[] assemblies)
{
    public Task<Dictionary<Type, TypeScriptType>> Resolve()
    {
        var typesWithAttribute = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            // IsPublic does not cover nested types
            .Where(type => !type.IsNotPublic && Attribute.IsDefined(type, typeof(TsGenerateAttribute)))
            .ToHashSet();

        var visitedTypes = new Dictionary<Type, TypeScriptType>();

        foreach (var type in typesWithAttribute)
        {
            CollectReferencedTypes(type, visitedTypes);
        }

        return Task.FromResult(visitedTypes);
    }

    private static CollectedTypeResult? CollectReferencedTypes(Type type, Dictionary<Type, TypeScriptType> visited)
    {
        var typeAlreadyVisited = visited.ContainsKey(type);

        if (typeAlreadyVisited)
        {
            return new(visited[type], false);
        }

        var isDictionary = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        var isArray = !isDictionary && IsArrayOrEnumerable(type);
        var typeToResolve = isArray ? GetArrayElementType(type) : type;

        if (typeof(Delegate).IsAssignableFrom(type))
        {
            return null;
        }

        if (typeToResolve.IsValueType || typeToResolve == typeof(string))
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
            return new(ResolveInterface(typeToResolve, visited), isArray);
        }

        throw new ArgumentException("Type is not supported", nameof(type));
    }

    private static TypeScriptInterface ResolveInterface(Type type, Dictionary<Type, TypeScriptType> visited)
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
                CollectReferencedTypes(openType, visited);
            }

            foreach (var genericTypeArgument in type.GetGenericArguments())
            {
                if (genericTypeArgument.IsGenericTypeParameter)
                {
                    typeScriptType.GenericArguments.Add(new TypeScriptGenericParameter(genericTypeArgument.Name));
                }
                else if (CollectReferencedTypes(genericTypeArgument, visited) is { } resolvedType)
                {
                    typeScriptType.GenericArguments.Add(resolvedType.TypeScriptType);
                }
            }
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

            if (CollectReferencedTypes(memberType, visited) is not { } resolvedType) continue;

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
            typeScriptType.BaseType = CollectReferencedTypes(type.BaseType, visited)?.TypeScriptType;
        }

        foreach (var implementedInterface in type.GetInterfaces())
        {
            if (CollectReferencedTypes(implementedInterface, visited) is { IsSystemType: false } resolvedType)
            {
                typeScriptType.ImplementedInterfaces.Add(resolvedType.TypeScriptType);
            }
        }

        visited.TryAdd(type, typeScriptType);

        return typeScriptType;
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