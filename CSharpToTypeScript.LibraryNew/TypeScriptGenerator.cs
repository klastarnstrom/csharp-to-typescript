using System.Reflection;
using CSharpToTypeScript.LibraryNew.Exports;

namespace CSharpToTypeScript.LibraryNew;

public class TypeScriptGenerator
{
    private readonly ExportResolver _exportResolver = new();
    
    public void Generate(Assembly[] assemblies)
    {
        var typesWithAttribute = new TypeCollector().CollectTypes(assemblies);
         
        foreach (var type in typesWithAttribute)
        {
            _exportResolver.ResolveType(type);
        }
        
        foreach (var export in _exportResolver.ExportContext.Exports)
        {
            Console.WriteLine($"export {export.ExportType} {export.TypeName.Name} {{");
            
            if (export is TsInterface tsInterface)
            {
                foreach (var property in tsInterface.Properties)
                {
                    Console.WriteLine($"    {property.Name}: {property.Type};");
                }
            }
            
            if (export is TsEnum tsEnum)
            {
                foreach (var value in tsEnum.Values)
                {
                    Console.WriteLine($"    {value} = \"{value}\",");
                }
            }
            
            Console.WriteLine("}\n");
        }
    }
}