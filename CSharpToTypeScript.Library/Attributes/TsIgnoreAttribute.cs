namespace CSharpToTypeScript.Library.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface)]
public class TsIgnoreAttribute : Attribute;