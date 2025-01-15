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
                await GenerateWriteArrayType(writer, arrayProperty);
                break;
            case TypeScriptDictionaryProperty dictionaryProperty:
                await GenerateDictionaryType(writer, dictionaryProperty);
                break;
            default:
                await TypeNameGenerator.Generate(writer, property.Type ?? throw new InvalidOperationException());
                break;
        }

        await writer.WriteAsync(";");
    }

    private static async Task GenerateWriteArrayType(StringWriter writer, TypeScriptArrayProperty arrayProperty)
    {
        await TypeNameGenerator.Generate(writer, arrayProperty.ElementType);
        await writer.WriteAsync("[]");
    }

    private static async Task GenerateDictionaryType(StringWriter writer,
        TypeScriptDictionaryProperty dictionaryProperty)
    {
        await writer.WriteAsync("{ [key: ");
        await TypeNameGenerator.Generate(writer, dictionaryProperty.KeyType);
        await writer.WriteAsync("]: ");
        await TypeNameGenerator.Generate(writer, dictionaryProperty.ValueType);
        await writer.WriteAsync(" }");
    }
}