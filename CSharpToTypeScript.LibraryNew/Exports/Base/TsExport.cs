namespace CSharpToTypeScript.LibraryNew.Exports;

public abstract class TsExport(Type type, ExportResolver exportResolver)
{
    public Type Type { get; } = type;
    public TsTypeName TypeName { get; } = new(type);
    public abstract string ExportType { get; }
}