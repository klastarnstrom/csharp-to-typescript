namespace CSharpToTypeScript.Library.Models;

public class TypeScriptEnum(string name, List<string> enumValues) : TypeScriptType(name)
{
    public  List<string> EnumValues { get; } = enumValues;
} 