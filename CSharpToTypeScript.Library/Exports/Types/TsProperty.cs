using System.Reflection;
using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Exports.Base;
using CSharpToTypeScript.Library.Extensions;

namespace CSharpToTypeScript.Library.Exports.Types;

public class TsProperty(MemberInfo memberInfo) : TsExport(memberInfo)
{
    private readonly MemberInfo _memberInfo = memberInfo;
    private bool IsMarkedAsNullable => _memberInfo.IsMarkedAsNullable();
    
    public override string Export()
    {
        var nullability = IsMarkedAsNullable ? "?" : "";
        
        return $"\t{_memberInfo.Name}{nullability}: {TypeName.Name};";
    }
}