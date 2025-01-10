namespace CSharpToTypeScript.Library.Models;

public class TypeScriptInterface : TypeScriptType
{
    public TypeScriptType? BaseType { get; set; }
    public List<TypeScriptType> ImplementedInterfaces { get; } = [];
    public bool IsGeneric => GenericArguments.Count > 0;
    public bool IsOpenGenericType { get; set; }
    public bool IsGenericParameter { get; set; }
    public List<TypeScriptType> GenericArguments { get; } = [];
    public List<TypeScriptProperty> Properties { get; } = [];
}