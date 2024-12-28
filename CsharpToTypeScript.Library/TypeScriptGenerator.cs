using CsharpToTypeScript.Library.Attributes;
using CsharpToTypeScript.Library.Resolvers;
using CsharpToTypeScript.Library.TypeGenerators;

namespace CsharpToTypeScript.Library;
// TODO: Handle Dictionary and List types
// TODO: Handle inheritance
public class TypeScriptGenerator(TypeScriptConfiguration? configuration = null)
{
    private readonly TypeScriptConfiguration _configuration = configuration ?? new ();
    private readonly TypesResolver _typesResolver = new();
    private readonly TypeDeclarationGenerator _typeDeclarationGenerator = new();
    private readonly EnumGenerator _enumGenerator = new();

    public async Task Generate()
    {
        await using var writer = new TypeScriptWriter(_configuration);
        
        writer.CreateOutputDirectory();
        writer.ClearFile();
        
        await writer.Comment($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => Attribute.IsDefined(type, typeof(TsGenerateAttribute)))
            .ToList();

        var resolvedTypes = _typesResolver.ResolveTypes(types);

        foreach (var typeDeclaration in resolvedTypes.Select(GenerateTypeString))
        {
            await writer.AddLine(typeDeclaration);
        }
    }

    private string GenerateTypeString(TypeMetadata metaData)
    {
        if (metaData.IsEnum)
        {
            return _enumGenerator.Generate(metaData);
        }
        
        return _typeDeclarationGenerator.Generate(metaData);
    }
}