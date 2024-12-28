namespace CsharpToTypeScript.Library.Resolvers.TypeResolvers.Base;

internal interface ITypeResolver
{
    TypeMetadata Resolve(Type type);
}