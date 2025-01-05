namespace CSharpToTypeScript.Library.Models;

public class TypeScriptInterface : TypeScriptType
{
    public TypeScriptType? BaseType { get; set; }
    public List<TypeScriptType> ImplementedInterfaces { get; } = [];
    public List<TypeScriptType> GenericArguments { get; } = [];
    public List<TypeScriptProperty> Properties { get; } = [];
}