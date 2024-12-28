namespace CsharpToTypeScript.Library.Resolvers.TypeResolvers.Base;

public class BaseTypeResolver
{
    protected static bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;
}