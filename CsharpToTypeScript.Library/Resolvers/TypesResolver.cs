using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using CsharpToTypeScript.Library.Resolvers.TypeResolvers;

namespace CsharpToTypeScript.Library.Resolvers;

internal class TypesResolver
{
    private readonly DataMemberResolver _dataMemberResolver = new();
    private readonly ArrayResolver _arrayResolver = new();
    private readonly EnumResolver _enumResolver = new();

    public ReadOnlyCollection<TypeMetadata> ResolveTypes(List<Type> types)
    {
        var resolvedTypes = new Dictionary<Type, TypeMetadata>();

        foreach (var type in types)
        {
            if (resolvedTypes.ContainsKey(type))
            {
                continue;
            }

            resolvedTypes.Add(type, ResolveType(type, resolvedTypes));
        }

        return resolvedTypes.Values.ToList().AsReadOnly();
    }

    private TypeMetadata ResolveType(Type type, Dictionary<Type, TypeMetadata> resolvedTypes)
    {
        if (type.IsEnum)
        {
           return _enumResolver.Resolve(type);
        }
        
        var isArray = IsArrayLike(type);

        if (isArray)
        {
            return _arrayResolver.Resolve(type);
        }
        
        var isClassOrInterface = type.IsInterface || type.IsClass;
        
        if (isClassOrInterface)
        {
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

                var isSystemType = dataMemberType.Namespace?.StartsWith("System") ?? false;

                if (isSystemType || resolvedTypes.ContainsKey(dataMemberType))
                {
                    continue;
                }

                dataMemberResults.Add(new ()
                {
                    Name = memberInfo.Name,
                    MetaData = ResolveType(dataMemberType, resolvedTypes)
                });
            }

            return new()
            {
                IsInterface = type.IsInterface,
                IsClass = type.IsClass,
                Name = type.Name,
                DataMembers = dataMemberResults
            };
        }

        throw new NotSupportedException($"Type {type} is not supported.");
    }

    private static bool IsArrayLike(Type propertyType) =>
        typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string);
}