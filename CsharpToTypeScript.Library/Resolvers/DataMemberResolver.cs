using System.Collections;
using System.Reflection;

namespace CsharpToTypeScript.Library.Resolvers;

internal record DataMemberResolveResult(
    string Name,
    string TypeName,
    bool IsNullable,
    bool IsArrayLike,
    bool IsDictionary);

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

        // If the type is an array, resolve the element type
        var isDictionary = IsDictionary(typeInfo);
        var isArrayLike = !isDictionary && IsArrayLike(typeInfo);
        
        var typeToResolve = GetTypeToResolve(typeInfo, isArrayLike);

        var memberName = member.Name;
        var typeName = _dataMemberTypeResolver.Resolve(typeToResolve);
        var isNullable = IsNullable(typeToResolve);

        return new(memberName, typeName, isNullable, isArrayLike, isDictionary);
    }

    private static bool IsNullable(Type propertyType) =>
        propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

    private static bool IsArrayLike(Type propertyType)
    {
        var isArrayLike = typeof(IEnumerable).IsAssignableFrom(propertyType);

        return isArrayLike && propertyType != typeof(string);
    }

    private static bool IsDictionary(Type propertyType) => typeof(IDictionary).IsAssignableFrom(propertyType);

    private static Type GetTypeToResolve(Type type, bool isArrayLike)
    {
        if (!isArrayLike)
        {
            return type;
        }

        // If type is a generic collection, return the generic argument
        if (!type.IsArray && type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType &&
                                                      interfaceType.GetGenericTypeDefinition()
                                                      == typeof(IEnumerable<>)))
        {
            return type.GetGenericArguments()[0];
        }

        // If type is regular array, return the element type
        return type.GetElementType() ?? throw new ArgumentException("Type is not an array", nameof(type));
    }
}