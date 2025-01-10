namespace CSharpToTypeScript.Library.Models.Properties;

public class TypeScriptArrayProperty(string name, TypeScriptType elementType, bool isNullable) : TypeScriptProperty(name, isNullable)
{
    public TypeScriptType ElementType { get; } = elementType;
}