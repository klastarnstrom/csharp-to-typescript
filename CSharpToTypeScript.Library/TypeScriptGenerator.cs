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

    public async Task Generate(TypeScriptConfiguration configuration)
    {
        var typesWithAttribute =
            TypeCollector.CollectTypes(configuration.Assemblies);
        
        foreach (var type in typesWithAttribute)
        {
            _exportResolver.ResolveType(type);
        }

        await using var writer = new TypeScriptWriter(configuration);

        var types = _exportResolver.ExportContext.Exports;

        await writer.WriteTypeScriptFile(_builtInTypes.Concat(types));
    }
}