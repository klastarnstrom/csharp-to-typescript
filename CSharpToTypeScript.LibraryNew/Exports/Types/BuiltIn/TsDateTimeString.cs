using CSharpToTypeScript.LibraryNew.Exports.Base;

namespace CSharpToTypeScript.LibraryNew.Exports.Types.BuiltIn;

public class TsDateTimeString : ITsExport
{
    public const string Name = "DateTimeString";
    public string Export() => $"export type {Name} = string;";
}