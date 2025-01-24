using CSharpToTypeScript.Library.Exports.Base;

namespace CSharpToTypeScript.Library.Exports.Types.BuiltIn;

public class TsDateString : ITsExport
{
    public const string Name = "DateString";
    public string Export() => $"export type {Name} = string;";
}