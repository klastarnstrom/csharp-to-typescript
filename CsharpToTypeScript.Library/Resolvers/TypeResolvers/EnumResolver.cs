namespace CsharpToTypeScript.Library.Resolvers;

internal class EnumResolver : ITypeResolver
{
    public TypeResolveResult Resolve(Type type)
    {
        return new EnumResolveResult(type.Name, Enum.GetNames(type));
    }
}