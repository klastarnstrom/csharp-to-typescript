using CSharpToTypeScript.LibraryNew.Exports.Base;

namespace CSharpToTypeScript.LibraryNew.Exports;

public class ExportContext
{
    public List<ITsExport> Exports { get; } = [];
    
    public void AddExport(TsExport export)
    {
        if (Exports.Any(e => e is TsExport tsExport && tsExport.Type == export.Type))
        {
            return;
        }
        
        Exports.Add(export);
    }
}