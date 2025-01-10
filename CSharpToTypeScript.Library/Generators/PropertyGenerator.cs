using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Models.Properties;

namespace CSharpToTypeScript.Library.Generators;

public static class PropertyGenerator
{
    public static async Task Generate(StringWriter writer, TypeScriptProperty property)
    {
         await writer.WriteAsync($"{SpecialCharacters.Tab}{property.Name}");

        if (property.IsNullable)
        {
             await writer.WriteAsync('?');
        }

         await writer.WriteAsync(": ");

        switch (property)
        {
            case TypeScriptArrayProperty arrayProperty:
                 await writer.WriteAsync($"{arrayProperty.ElementType.Name}[]");
                break;
            case TypeScriptDictionaryProperty dictionaryProperty:
                 await writer.WriteAsync($"{{ [key: {dictionaryProperty.KeyType.Name}]: {dictionaryProperty.ValueType.Name} }}");
                break;
            default:
                 await writer.WriteAsync($"{property.Type?.Name}");
                break;
        }

         await writer.WriteLineAsync(";");
    }
}