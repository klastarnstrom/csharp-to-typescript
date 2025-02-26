using System.Reflection;
using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Exports.Types;

namespace CSharpToTypeScript.Library.Exports.Base;

public interface ITsExport
{
    public string Export();
}

public abstract class TsExport : ITsExport
{
    public Type Type { get; }
    protected StringWriter Builder { get; } = new() { NewLine = SpecialCharacters.UnixNewLine };
    protected TsTypeName TypeName { get; }
    public abstract string Export();

    protected TsExport(MemberInfo memberInfo)
    {
        Type = memberInfo switch
        {
            PropertyInfo propertyInfo => propertyInfo.PropertyType,
            FieldInfo fieldInfo => fieldInfo.FieldType,
            _ => throw new NotSupportedException(ErrorMessage.MemberIsNotSupported(memberInfo))
        };
        
        TypeName = new(Type);
    }
    
    protected TsExport(Type type)
    {
        Type = type;
        TypeName = new(type);
    }
}