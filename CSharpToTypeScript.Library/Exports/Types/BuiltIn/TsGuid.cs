using CSharpToTypeScript.Library.Exports.Base;

namespace CSharpToTypeScript.Library.Exports.Types.BuiltIn;

public class TsGuid : ITsExport
{
    public const string Name = "Guid";
    public string Export() => $"export type {Name} = string;";
}