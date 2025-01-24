using System.Reflection;

namespace CSharpToTypeScript.LibraryNew.Exports.Types;

public class TsProperty(MemberInfo memberInfo)
{
    public string Name => memberInfo.Name;
    
    public string Type => new TsTypeName(PropertyType).Name;
    
    public Type PropertyType => memberInfo switch
    {
        PropertyInfo propertyInfo => propertyInfo.PropertyType,
        FieldInfo fieldInfo => fieldInfo.FieldType,
        _ => throw new NotSupportedException($"MemberInfo {memberInfo.Name} is not supported")
    };
}