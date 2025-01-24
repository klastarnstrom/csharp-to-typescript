using System.Reflection;
using CSharpToTypeScript.LibraryNew.Attributes;

namespace CSharpToTypeScript.LibraryNew.Extensions;

public static class MemberInfoExtensions
{
    public static bool IsIgnored(this MemberInfo memberInfo)
    {
        if (memberInfo.GetCustomAttribute<TsIgnoreAttribute>() != null)
        {
            return true;
        }
        
        return memberInfo.DeclaringType?.IsIgnored() == true;
    }
}