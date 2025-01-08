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
            return new(visited[type]);
        }

        var isArray = IsArrayOrEnumerable(type);
        var typeToResolve = isArray ? GetArrayElementType(type) : type;

        if (typeof(Delegate).IsAssignableFrom(type))
        {
            return null;
        }

        if (IsSystemType(typeToResolve))
        {
            return new(TypeScriptSystemType.Create(typeToResolve), isArray)
            {
                IsSystemType = true
            };
        }

        if (typeToResolve.IsEnum)
        {
            var tsEnum = new TypeScriptEnum
            {
                Name = typeToResolve.Name,
                EnumValues = Enum.GetNames(typeToResolve).ToList(),
            };

            visited.Add(typeToResolve, tsEnum);

            return new(tsEnum);
        }

        if (typeToResolve.IsInterface || typeToResolve.IsClass)
        {
            return new(ResolveInterface(typeToResolve, visited), isArray);
        }

        throw new ArgumentException("Type is not supported", nameof(type));
    }

    private static TypeScriptInterface ResolveInterface(Type type, Dictionary<Type, TypeScriptType> visited)
    {
        var typeScriptType = new TypeScriptInterface
        {
            Name = type.Name,
        };

        if (type.IsGenericType)
        {
            foreach (var genericTypeArgument in type.GetGenericArguments())
            {
                if (CollectReferencedTypes(genericTypeArgument, visited) is { } resolvedType)
                {
                    typeScriptType.GenericArguments.Add(resolvedType.TypeScriptType);
                }
            }
        }

        var properties = type.GetProperties().Where(p => p.DeclaringType == type && !IsIgnored(p));

        foreach (var property in properties)
        {
            if (CollectReferencedTypes(property.PropertyType, visited) is { } resolvedType)
            {
                typeScriptType.Properties.Add(new(property.Name, resolvedType.TypeScriptType,
                    resolvedType.IsArrayElementType));
            }
        }

        var fields = type.GetFields().Where(f =>
            f.DeclaringType == type && !IsIgnored(f));

        foreach (var field in fields)
        {
            var resolvedType = CollectReferencedTypes(field.FieldType, visited);

            if (resolvedType is not null)
            {
                typeScriptType.Properties.Add(new(field.Name, resolvedType.TypeScriptType,
                    resolvedType.IsArrayElementType));
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