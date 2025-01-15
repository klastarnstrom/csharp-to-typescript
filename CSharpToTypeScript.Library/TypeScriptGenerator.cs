namespace CSharpToTypeScript.Library;

public class TypeScriptGenerator(TypeScriptConfiguration? configuration = null)
{
    private readonly TypeScriptConfiguration _configuration = configuration ?? new();

    public async Task Generate()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        await using var writer = new TypeScriptWriter(_configuration);
        
        // Collect all types from the assemblies that have the TsGenerate attribute as well as all nested types
        var collectedTypes = await new TypeCollector(assemblies).Collect();
        
        await writer.WriteTypeScriptFile(collectedTypes);
    }
}