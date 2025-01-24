using System.Reflection;
using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Extensions;

namespace CSharpToTypeScript.Library.Exports.Types;

public class TsProperty(MemberInfo memberInfo)
{
    public string Name => memberInfo.Name;
    
    public string Type => new TsTypeName(PropertyType).Name;
    
    public bool IsMarkedAsNullable => memberInfo.IsMarkedAsNullable();
    
    public Type PropertyType => memberInfo switch
    {
        PropertyInfo propertyInfo => propertyInfo.PropertyType,
        FieldInfo fieldInfo => fieldInfo.FieldType,
        _ => throw new NotSupportedException(ErrorMessage.MemberIsNotSupported(memberInfo))
    };
}