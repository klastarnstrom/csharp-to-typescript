using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using CsharpToTypeScript.Library.Resolvers.TypeResolvers;

namespace CsharpToTypeScript.Library.Resolvers;

internal class TypesResolver
{
    private readonly EnumResolver _enumResolver = new();

    public ReadOnlyCollection<TypeMetadata> ResolveTypes(List<Type> types)
    {
        var resolvedTypes = new Dictionary<Type, TypeMetadata>();

        foreach (var type in types)
        {
            var isSystemType = type.Namespace?.StartsWith("System") ?? false;

            if (isSystemType || resolvedTypes.ContainsKey(type))
            {
                continue;
            }

            if (resolvedTypes.ContainsKey(type))
            {
                continue;
            }

            var resolvedType = ResolveType(type);

            foreach (var typeMetadata in resolvedType)
            {
                resolvedTypes.TryAdd(typeMetadata.Type, typeMetadata);
            }
        }

        return resolvedTypes.Values.ToList().AsReadOnly();
    }

    private IEnumerable<TypeMetadata> ResolveType(Type type)
    {
        if (type.IsEnum)
        {
            yield return _enumResolver.Resolve(type);
        }

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        var dataMemberInfos = properties.Cast<MemberInfo>().Concat(fields);

        List<TypeDataMember> dataMemberResults = [];

        foreach (var memberInfo in dataMemberInfos)
        {
            var dataMemberType = memberInfo switch
            {
                FieldInfo field => field.FieldType,
                PropertyInfo property => property.PropertyType,
                _ => throw new ArgumentException("Member is not a field or property", nameof(memberInfo))
            };

            var dataMemberResolvedType = ResolveType(dataMemberType).First();
            
            var isArrayOrEnumerable = IsArrayOrEnumerable(dataMemberType);

            var dataMember = new TypeDataMember
            {
                Name = memberInfo.Name,
                IsArray =  isArrayOrEnumerable,
                MetaData = dataMemberResolvedType
            };

            yield return dataMemberResolvedType;

            dataMemberResults.Add(dataMember);
        }

        yield return new()
        {
            Type = type,
            IsInterface = type.IsInterface,
            IsClass = type.IsClass,
            Name = type.Name,
            DataMembers = dataMemberResults
        };
    }

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