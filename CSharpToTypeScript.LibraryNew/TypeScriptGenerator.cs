using CSharpToTypeScript.LibraryNew.Attributes;
using CSharpToTypeScript.LibraryNew.Exports;
using CSharpToTypeScript.LibraryNew.Exports.Base;
using CSharpToTypeScript.LibraryNew.Exports.Types.BuiltIn;

namespace CSharpToTypeScript.LibraryNew;

public class TypeScriptGenerator
{
    private readonly ExportResolver _exportResolver = new();

    private readonly ITsExport[] _builtInTypes =
    [
        new TsDateTimeString(),
        new TsDateString(),
        new TsGuid()
    ];

    public void Generate(TypeScriptConfiguration configuration)
    {
        var typesWithAttribute =
            TypeCollector.CollectTypes(configuration.Assemblies);
        
        foreach (var type in typesWithAttribute)
        {
            _exportResolver.ResolveType(type);
        }

        using var writer = new TypeScriptWriter(configuration);

        var types = _exportResolver.ExportContext.Exports;

        writer.WriteTypeScriptFile(_builtInTypes.Concat(types));
    }
}