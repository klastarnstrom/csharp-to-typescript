using CSharpToTypeScript.Library.Models.Properties;

namespace CSharpToTypeScript.Library.Generators;

public static class BodyGenerator
{
    public static async Task Generate(StringWriter writer, List<TypeScriptProperty> properties)
    {
        await writer.WriteAsync(" {");
        
        if (properties.Count > 0)
        {
            await writer.WriteLineAsync();
            
            foreach (var property in properties)
            {
               await PropertyGenerator.Generate(writer, property);
            }
        }

        await writer.WriteAsync("}");
    }
}