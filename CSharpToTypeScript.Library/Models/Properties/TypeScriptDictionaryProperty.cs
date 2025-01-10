namespace CSharpToTypeScript.Library.Models.Properties;

public class TypeScriptDictionaryProperty(
    string name,
    TypeScriptType keyType,
    TypeScriptType valueType,
    bool isNullable) : TypeScriptProperty(name, isNullable)
{
    public TypeScriptType KeyType { get; } = keyType;
    public TypeScriptType ValueType { get; } = valueType;
}