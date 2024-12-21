namespace CsharpToTypeScript.Library.Resolvers;

public abstract class DataMemberResolverBase
{
    protected static bool IsNullable(Type propertyType) =>
        propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
}