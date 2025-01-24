using System.Reflection;

namespace CSharpToTypeScript.LibraryNew.Constants;

public static class ErrorMessage
{
    public static string GenericArgument(Type type) => $"Generic argument type '{type.FullName}' is ignored";

    public static string MemberIsNotSupported(MemberInfo memberInfo) =>
        $"Member {memberInfo.DeclaringType?.Name}.{memberInfo.Name} is not a property or field";
    
    public static string PropertyTypeIsIgnored(Type type) => $"Property type '{type.FullName}' is ignored";

    public const string TypeIsNotAnArray = "Type is not an array";
}