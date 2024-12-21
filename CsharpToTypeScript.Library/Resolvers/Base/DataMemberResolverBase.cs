using System.Collections;

namespace CsharpToTypeScript.Library.Resolvers.Base;

internal record DataMemberResolveResult(string Name, string Type, bool IsNullable, bool IsArray);

internal abstract class DataMemberResolverBase
{
    protected static bool IsNullable(Type propertyType) =>
        propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

    protected static bool IsArray(Type propertyType) => typeof(IEnumerable).IsAssignableFrom(propertyType);
}