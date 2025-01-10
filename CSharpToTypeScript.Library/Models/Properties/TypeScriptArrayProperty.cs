namespace CSharpToTypeScript.Library.Models;

public class TypeScriptArrayProperty(string name, TypeScriptType elementType, bool isNullable) : TypeScriptProperty(name, isNullable)
{
    public TypeScriptType ElementType { get; } = elementType;
}