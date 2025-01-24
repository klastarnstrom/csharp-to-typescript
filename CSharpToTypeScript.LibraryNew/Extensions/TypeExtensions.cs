using CSharpToTypeScript.LibraryNew.Constants;

namespace CSharpToTypeScript.LibraryNew.Extensions;

public static class TypeExtensions
{
    public static bool IsDictionary(this Type type) => typeof(Dictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition());

    public static bool IsEnumerable(this Type type)
    {
        if (type == typeof(string))
        {
            return false;
        }

        if (type.IsArray)
        {
            return true;
        }

        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
               type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }
    
    public static Type GetEnumerableElementType(this Type type)
    {
        // If type implements IEnumerable<> return the generic type argument
        if (type.IsGenericType && IsEnumerable(type))
        {
            return type.GetGenericArguments()[0];
        }

        // If type is regular array, return the element type
        return type.GetElementType() ?? throw new ArgumentException(ErrorMessage.TypeIsNotAnArray, nameof(type));
    }
    
    public static bool IsSystemType(this Type type) => type.FullName?.StartsWith("System.") == true;
}