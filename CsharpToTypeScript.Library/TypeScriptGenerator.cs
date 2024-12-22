using CsharpToTypeScript.Library.Attributes;
using CsharpToTypeScript.Library.Constants;
using CsharpToTypeScript.Library.Resolvers;
using CsharpToTypeScript.Library.TypeGenerators;

namespace CsharpToTypeScript.Library;
// TODO: Handle Dictionary and List types
// TODO: Handle inheritance
public class TypeScriptGenerator(TypeScriptConfiguration? configuration = null)
{
    private readonly TypeScriptConfiguration _configuration = configuration ?? new ();
    private readonly TypesResolver _typesResolver = new();
    private readonly InterfaceGenerator _interfaceGenerator = new();
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

    private string GenerateTypeString(TypeResolveResult result) =>
        result.GenerateType switch
        {
            GenerateType.Interface when result is InterfaceResolveResult interfaceResult =>
                _interfaceGenerator.Generate(interfaceResult),
            GenerateType.Enum when result is EnumResolveResult enumResult => _enumGenerator.Generate(enumResult),
            _ => throw new NotSupportedException($"Type {result.GenerateType} is not supported.")
        };
}