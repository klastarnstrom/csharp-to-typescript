using CsharpToTypeScript.Library.Resolvers.TypeResolvers.Base;

namespace CsharpToTypeScript.Library.Resolvers.TypeResolvers;

internal class EnumResolver : ITypeResolver
{
    public TypeMetadata Resolve(Type type) =>
        new()
        {
            Name = type.Name,
            IsEnum = true,
            EnumValues = Enum.GetNames(type).ToList(),
        };
}