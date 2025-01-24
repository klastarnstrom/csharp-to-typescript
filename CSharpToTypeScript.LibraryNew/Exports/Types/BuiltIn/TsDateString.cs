using CSharpToTypeScript.LibraryNew.Exports.Base;

namespace CSharpToTypeScript.LibraryNew.Exports.Types.BuiltIn;

public class TsDateString : ITsExport
{
    public const string Name = "DateString";
    public string Export() => $"export type {Name} = string;";
}