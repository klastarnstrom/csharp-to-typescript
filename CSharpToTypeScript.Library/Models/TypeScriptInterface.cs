using CSharpToTypeScript.Library.Models.Properties;

namespace CSharpToTypeScript.Library.Models;

public class TypeScriptInterface(string name) : TypeScriptType(name)
{
    public TypeScriptType? BaseType { get; set; }
    public List<TypeScriptType> ImplementedInterfaces { get; } = [];
    public bool IsGeneric => GenericArguments.Count > 0;
    public bool IsOpenGenericType { get; set; }
    public bool IsGenericParameter { get; set; }
    public List<TypeScriptType> GenericArguments { get; } = [];
    public List<TypeScriptProperty> Properties { get; } = [];
}