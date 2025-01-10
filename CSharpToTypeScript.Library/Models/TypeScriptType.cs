namespace CSharpToTypeScript.Library.Models;

public abstract class TypeScriptType(string name)
{
    public string Name { get; } = name;
}