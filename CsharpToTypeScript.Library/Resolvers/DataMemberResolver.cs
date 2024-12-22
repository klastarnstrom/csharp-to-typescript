using System.Collections;
using System.Reflection;

namespace CsharpToTypeScript.Library.Resolvers;

internal record DataMemberResolveResult(string Name, string Type, bool IsNullable, bool IsArray, bool IsDictionary);

internal class DataMemberResolver
{
    private readonly DataMemberTypeResolver _dataMemberTypeResolver = new();

    internal DataMemberResolveResult Resolve(MemberInfo member)
    {
        var typeInfo = member switch
        {
            FieldInfo field => field.FieldType,
            PropertyInfo property => property.PropertyType,
            _ => throw new ArgumentException("Member is not a field or property", nameof(member))
        };

        var name = member.Name;
        var typeName = _dataMemberTypeResolver.Resolve(typeInfo);
        var isNullable = IsNullable(typeInfo);
        var isArray = IsArray(typeInfo);
        var isDictionary = IsDictionary(typeInfo);

        return new(name, typeName, isNullable, isArray, isDictionary);
    }

    private static bool IsNullable(Type propertyType) =>
        propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
    private static bool IsArray(Type propertyType) => typeof(IEnumerable).IsAssignableFrom(propertyType);
    private static bool IsDictionary(Type propertyType) => typeof(IDictionary).IsAssignableFrom(propertyType);
    protected static bool IsComplexType(Type propertyType) => propertyType.IsClass || propertyType.IsInterface;
}