using System.Collections.ObjectModel;
using System.Reflection;

namespace CsharpToTypeScript.Library.Resolvers;

internal class TypesResolver
{
    private readonly DataMemberResolver _dataMemberResolver = new();
    private readonly EnumResolver _enumResolver = new();

    public ReadOnlyCollection<TypeResolveResult> ResolveTypes(List<Type> types)
    {
        var resolvedTypes = new Dictionary<Type, TypeResolveResult>();

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

    private TypeResolveResult ResolveType(Type type, Dictionary<Type, TypeResolveResult> resolvedTypes)
    {
        if (type.IsEnum)
        {
            return new EnumResolveResult(type.Name, Enum.GetNames(type));
        }

        if (type.IsInterface || type.IsClass)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            var dataMemberInfos = properties.Cast<MemberInfo>().Concat(fields);

            List<DataMemberResolveResult> dataMemberResults = [];

            foreach (var memberInfo in dataMemberInfos)
            {
                var dataMemberType = memberInfo switch
                {
                    FieldInfo field => field.FieldType,
                    PropertyInfo property => property.PropertyType,
                    _ => throw new ArgumentException("Member is not a field or property", nameof(memberInfo))
                };

                dataMemberResults.Add(_dataMemberResolver.Resolve(memberInfo));

                var isSystemType = dataMemberType.Namespace?.StartsWith("System") ?? false;
                
                if (isSystemType || resolvedTypes.ContainsKey(dataMemberType))
                {
                    continue;
                }

                if (dataMemberType.IsEnum)
                {
                    resolvedTypes.Add(dataMemberType, _enumResolver.Resolve(dataMemberType));
                }
                else if (dataMemberType.IsInterface || dataMemberType.IsClass)
                {
                    resolvedTypes.Add(dataMemberType, ResolveType(dataMemberType, resolvedTypes));
                }
            }

            return new InterfaceResolveResult(type.Name, dataMemberResults);
        }

        throw new NotSupportedException($"Type {type} is not supported.");
    }
}