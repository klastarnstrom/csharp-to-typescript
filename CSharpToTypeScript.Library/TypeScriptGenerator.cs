using CSharpToTypeScript.Library.Exports;
using CSharpToTypeScript.Library.Exports.Base;
using CSharpToTypeScript.Library.Exports.Types.BuiltIn;
using CSharpToTypeScript.Library.Attributes;

namespace CSharpToTypeScript.Library;

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