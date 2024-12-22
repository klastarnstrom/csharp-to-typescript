namespace CsharpToTypeScript.Library.Resolvers;

internal interface ITypeResolver
{
    TypeResolveResult Resolve(Type type);
}