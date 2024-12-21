using System.Reflection;
using CsharpToTypeScript.Library.Resolvers;
using CsharpToTypeScript.Library.TypeGenerators;
using CsharpToTypeScript.Library.TypeGenerators.Base;

namespace CsharpToTypeScript.Library;

public class TypeScriptGenerator(TypeScriptConfiguration configuration)
{
    private readonly InterfaceGenerator _interfaceGenerator = new();
    private readonly EnumGenerator _enumGenerator = new();

    public void Generate()
    {
        // Create output directory if it does not exist
        var outputPath = $"c:/temp/{configuration.OutputPath}";
        
        Directory.CreateDirectory(outputPath);

        var filePath = Path.Combine(outputPath, $"{configuration.FileName}");
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
        using var fileStream = File.Create(filePath);
        using var writer = new StreamWriter(fileStream);

        foreach (var includedNamespace in configuration.IncludedNamespaces)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a =>
                {
                    var name = a.GetName().Name;

                    if (string.IsNullOrEmpty(name))
                    {
                        return false;
                    }

                    return !name.StartsWith("System.")
                           && !name.StartsWith("Microsoft.")
                           && !name.StartsWith("netstandard")
                           && !name.StartsWith("mscorlib")
                           && name != Assembly.GetExecutingAssembly().GetName().Name;
                });

            var allTypes = assemblies.SelectMany(a => a.GetTypes());
            var types = allTypes
                .Where(t => t.Namespace == includedNamespace)
                .ToList();

            foreach (var type in types)
            {
                var typeDeclaration = GenerateType(type);
                writer.WriteLine();
                writer.Write(typeDeclaration);
            }
        }
    }

    private string GenerateType(Type type)
    {
        if (type.IsEnum)
        {
            return _enumGenerator.Generate(type);
        }
        
        var typeDeclaration = _interfaceGenerator.Generate(type);
        
        return typeDeclaration;
    }
}