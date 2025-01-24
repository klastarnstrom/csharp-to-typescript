using CSharpToTypeScript.LibraryNew.Exports.Base;

namespace CSharpToTypeScript.LibraryNew.Exports.Types.BuiltIn;

public class TsGuid : ITsExport
{
    public const string Name = "Guid";
    public string Export() => $"export type {Name} = string;";
}