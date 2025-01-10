using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library.Generators;

internal static class EnumGenerator
{
    public static async Task<string> Generate(TypeScriptEnum typeScriptEnum)
    {
        var writer = new TypeScriptStringWriter();

        await writer.WriteLineAsync($"enum {typeScriptEnum.Name} {{");

        foreach (var value in typeScriptEnum.EnumValues)
        {
            await writer.WriteLineAsync($"{SpecialCharacters.Tab}{value} = \"{value}\",");
        }

        await writer.WriteAsync("}");

        return writer.ToString();
    }
}