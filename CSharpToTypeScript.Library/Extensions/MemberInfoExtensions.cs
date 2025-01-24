using System.Reflection;
using CSharpToTypeScript.Library.Attributes;

namespace CSharpToTypeScript.Library.Extensions;

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
    
    public static bool IsMarkedAsNullable(this MemberInfo memberInfo)
    {
        var nullabilityContext = new NullabilityInfoContext();

        var nullabilityInfo = memberInfo switch
        {
            PropertyInfo propertyInfo => nullabilityContext.Create(propertyInfo),
            FieldInfo fieldInfo => nullabilityContext.Create(fieldInfo),
            _ => throw new InvalidOperationException("Member is not property or field")
        };

        return nullabilityInfo.WriteState is NullabilityState.Nullable;
    }
}