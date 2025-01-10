namespace CSharpToTypeScript.Library.Models;

public class CollectedTypeResult(TypeScriptType typeScriptType, bool isSystemType, bool isArrayElementType = false)
{
    public TypeScriptType TypeScriptType { get; } = typeScriptType;
    public bool IsSystemType { get; } = isSystemType;
    public bool IsArrayElementType { get; } = isArrayElementType;
}