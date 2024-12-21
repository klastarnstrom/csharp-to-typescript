using System.Reflection;

namespace CsharpToTypeScript.Library;

public class TypeScriptGenerator
{
    private readonly TypeScriptConfiguration _configuration;
    private readonly DataMemberResolver _dataMemberResolver;

    public TypeScriptGenerator(TypeScriptConfiguration configuration)
    {
        _configuration = configuration;
        _dataMemberResolver = new();
    }

    public void Generate(Type type)
    {
        var outputPath = Path.Combine(_configuration.OutputPath, $"{type.Name}.ts");

        using var writer = new StreamWriter(outputPath);
        
        var typeDeclaration = type switch 
        {
            { IsClass: true } => "class",
            { IsInterface: true } => "interface",
            { IsEnum: true } => "enum",
            _ => throw new NotSupportedException($"Type {type.Name} is not supported.")
        };
        
        writer.WriteLine($"{typeDeclaration} {type.Name} {{");
        
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var members = fields.Concat<MemberInfo>(properties);
        
        foreach (var member in members)
        {
            var memberResolveResult = _dataMemberResolver.Resolve(member);
        }

        writer.WriteLine("}");
    }
}