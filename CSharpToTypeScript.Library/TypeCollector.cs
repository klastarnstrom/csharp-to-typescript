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
            // IsPublic does not cover all public cases
            .Where(type => !type.IsNotPublic && Attribute.IsDefined(type, typeof(TsGenerateAttribute)))
            .ToHashSet();

        var visitedTypes = new Dictionary<Type, TypeScriptType>();

        foreach (var type in typesWithAttribute)
        {
            CollectReferencedTypes(type, visitedTypes);
        }

        return Task.FromResult(visitedTypes);
    }

    private static CollectedTypeResult CollectReferencedTypes(Type type, Dictionary<Type, TypeScriptType> visited)
    {
        var typeAlreadyVisited = visited.ContainsKey(type);

        if (typeAlreadyVisited)
        {
            return new(visited[type]);
        }

        var isArray = IsArrayOrEnumerable(type);
        var typeToResolve = isArray ? GetArrayElementType(type) : type;

        if (typeof(Delegate).IsAssignableFrom(typeToResolve))
        {
            throw new ArgumentException("Delegates are not supported", nameof(type));
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
                typeScriptType.GenericArguments.Add(CollectReferencedTypes(genericTypeArgument, visited)
                    .TypeScriptType);
            }
        }

        var properties = type.GetProperties().Where(p => p.DeclaringType == type);

        foreach (var property in properties)
        {
            var resolvedType = CollectReferencedTypes(property.PropertyType, visited);

            typeScriptType.Properties.Add(new(property.Name, resolvedType.TypeScriptType,
                resolvedType.IsArrayElementType));
        }

        var fields = type.GetFields().Where(f => f.DeclaringType == type);

        foreach (var field in fields)
        {
            var resolvedType = CollectReferencedTypes(field.FieldType, visited);

            typeScriptType.Properties.Add(new(field.Name, resolvedType.TypeScriptType,
                resolvedType.IsArrayElementType));
        }

        if (type.BaseType != null && !IsSystemType(type.BaseType))
        {
            typeScriptType.BaseType = CollectReferencedTypes(type.BaseType, visited).TypeScriptType;
        }

        foreach (var implementedInterface in type.GetInterfaces())
        {
            var resolvedType = CollectReferencedTypes(implementedInterface, visited);

            if (!resolvedType.IsSystemType)
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
}