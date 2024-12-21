using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using CsharpToTypeScript.Library.Attributes;
using CsharpToTypeScript.Library.Constants;
using CsharpToTypeScript.Library.TypeGenerators;

namespace CsharpToTypeScript.Library;

public class TypeScriptGenerator(TypeScriptConfiguration? configuration = null)
{
    private readonly TypeScriptConfiguration _configuration = configuration ?? new();
    private readonly InterfaceGenerator _interfaceGenerator = new();
    private readonly EnumGenerator _enumGenerator = new();

    public void Generate()
    {
        var outputPath = $"c:/temp/{_configuration.OutputPath}";

        Directory.CreateDirectory(outputPath);

        var filePath = Path.Combine(outputPath, $"{_configuration.FileName}");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        using var writer = new StreamWriter(filePath);

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => Attribute.IsDefined(type, typeof(GenerateTypeScriptAttribute)))
            .ToList();

        var resolvedTypes = ResolveComplexTypes(types);

        foreach (var typeDeclaration in resolvedTypes.Select(GenerateTypeString))
        {
            writer.WriteLine();
            writer.Write(typeDeclaration);
        }
    }


    private static ReadOnlyCollection<TypeResolveResult> ResolveComplexTypes(List<Type> types)
    {
        var resolvedTypes = new List<TypeResolveResult>();

        foreach (var type in types)
        {
            if (resolvedTypes.Any(x => x.FullName == type.FullName))
            {
                continue;
            }
            
            var fullName = type.FullName ?? throw new InvalidOperationException("Type full name is null.");

            
            if (type.IsInterface || type.IsClass)
            {
                resolvedTypes.Add(new InterfaceResolveResult(type.Name, fullName, []));
            }
            else if (type.IsEnum)
            {
                var values = Enum.GetNames(type).ToList();

                resolvedTypes.Add(new EnumResolveResult(type.Name, fullName, values));
            }
            
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        return resolvedTypes.AsReadOnly();
    }

    private string GenerateTypeString(TypeResolveResult result) =>
        result.GenerateType switch
        {
            GenerateType.Interface when result is InterfaceResolveResult interfaceResult => _interfaceGenerator.Generate(interfaceResult),
            GenerateType.Enum when result is EnumResolveResult enumResult => _enumGenerator.Generate(enumResult),
            _ => throw new NotSupportedException($"Type {result.GenerateType} is not supported.")
        };
}