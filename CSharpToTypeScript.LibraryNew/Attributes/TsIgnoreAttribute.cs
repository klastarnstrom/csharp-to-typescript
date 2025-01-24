namespace CSharpToTypeScript.LibraryNew.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface |
                AttributeTargets.Property | AttributeTargets.Field)]
public class TsIgnoreAttribute : TsAttributeBase;