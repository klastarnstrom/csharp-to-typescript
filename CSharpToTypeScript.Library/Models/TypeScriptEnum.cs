namespace CSharpToTypeScript.Library.Models;

public class TypeScriptEnum : TypeScriptType
{
    public required List<string> EnumValues { get; init; } = [];
} 