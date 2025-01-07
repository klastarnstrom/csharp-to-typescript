namespace CSharpToTypeScript.Library.Models;

public record CollectedTypeResult(
    TypeScriptType TypeScriptType,
    bool IsArrayElementType = false)
{
    public bool IsSystemType { get; init; }
}