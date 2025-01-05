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
            .Where(type => Attribute.IsDefined(type, typeof(TsGenerateAttribute)))
            .ToHashSet();

        var visitedTypes = new Dictionary<Type, TypeScriptType>();

        foreach (var type in typesWithAttribute)
        {
            CollectReferencedTypes(type, visitedTypes);
        }

        return Task.FromResult(visitedTypes);
    }

    private static TypeScriptType? CollectReferencedTypes(Type type, Dictionary<Type, TypeScriptType> visited)
    {
        var typeAlreadyVisited = visited.ContainsKey(type);

        if (typeAlreadyVisited)
        {
            return visited[type];
        }

        if (IsSystemType(type))
        {
            return null;
        }

        var typeToResolve = IsArrayOrEnumerable(type) ? GetArrayElementType(type) : type;

        if (typeToResolve.IsEnum)
        {
            var tsEnum = new TypeScriptEnum
            {
                Name = typeToResolve.Name,
                EnumValues = Enum.GetNames(typeToResolve).ToList(),
            };

            visited.Add(typeToResolve, tsEnum);

            return tsEnum;
        }

        if (typeToResolve.IsInterface || typeToResolve.IsClass)
        {
            return ResolveInterface(typeToResolve, visited);
        }

        throw new ArgumentException("Type is not supported", nameof(type));
    }

    private static TypeScriptType? ResolveInterface(Type type, Dictionary<Type, TypeScriptType> visited)
    {
        var typeScriptType = new TypeScriptInterface
        {
            Name = type.Name,
        };

        if (type.IsGenericType)
        {
            foreach (var genericTypeArgument in type.GetGenericArguments())
            {
                var resolvedType = CollectReferencedTypes(genericTypeArgument, visited);

                if (resolvedType != null)
                {
                    typeScriptType.GenericArguments.Add(resolvedType);
                }
            }
        }

        foreach (var property in type.GetProperties())
        {
            var resolvedType = CollectReferencedTypes(property.PropertyType, visited);

            if (resolvedType != null)
            {
                typeScriptType.Properties.Add(new()
                {
                    Name = property.Name,
                    Type = resolvedType,
                });
            }
        }

        foreach (var field in type.GetFields())
        {
            var resolvedType = CollectReferencedTypes(field.FieldType, visited);

            if (resolvedType != null)
            {
                typeScriptType.Properties.Add(new()
                {
                    Name = field.Name,
                    Type = resolvedType,
                });
            }
        }

        if (type.BaseType != null)
        {
            var resolvedType = CollectReferencedTypes(type.BaseType, visited);
            
            if (resolvedType != null)
            {
                typeScriptType.BaseType = resolvedType;
            }
        }

        foreach (var implementedInterface in type.GetInterfaces())
        {
            var resolvedType = CollectReferencedTypes(implementedInterface, visited);
            
            if (resolvedType != null)
            {
                typeScriptType.ImplementedInterfaces.Add(resolvedType);
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
        if (type.IsArray)
        {
            return true;
        }

        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
               type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }
}