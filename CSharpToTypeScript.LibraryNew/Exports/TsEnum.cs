namespace CSharpToTypeScript.LibraryNew.Exports;

public class TsEnum(Type type, ExportResolver exportResolver) : TsExport(type, exportResolver)
{
    public override string ExportType => "enum";
    public string[] Values { get; } = Enum.GetNames(type);
}