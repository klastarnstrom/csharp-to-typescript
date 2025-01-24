using CSharpToTypeScript.Library.Exports.Base;

namespace CSharpToTypeScript.Library.Exports.Types.BuiltIn;

public class TsDateTimeString : ITsExport
{
    public const string Name = "DateTimeString";
    public string Export() => $"export type {Name} = string;";
}