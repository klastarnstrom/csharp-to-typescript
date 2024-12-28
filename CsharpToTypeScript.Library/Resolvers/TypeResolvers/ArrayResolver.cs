using CsharpToTypeScript.Library.Resolvers.TypeResolvers.Base;

namespace CsharpToTypeScript.Library.Resolvers.TypeResolvers;

public class ArrayResolver : BaseTypeResolver, ITypeResolver
{
    public TypeMetadata Resolve(Type type)
    {
        var elementType = GetArrayElementType(type);
        
        return new()
        {
            Name = type.Name,
            IsArray = true,
            ArrayElementType = elementType,
            IsNullable = IsNullable(type)
        };
    }
    
    private static Type GetArrayElementType(Type type)
    {
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