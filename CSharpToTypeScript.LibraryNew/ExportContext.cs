using CSharpToTypeScript.LibraryNew.Exports;

namespace CSharpToTypeScript.LibraryNew;

public class ExportContext
{
    public List<TsExport> Exports { get; } = [];
    
    public void AddExport(TsExport export)
    {
        if (Exports.Any(e => e.Type == export.Type))
        {
            throw new InvalidOperationException($"Export for type {export.Type.Name} already exists");
        }
        
        Exports.Add(export);
    }
}