using System.Text;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library.Generators;

internal static class EnumGenerator
{
    public static Task<string> Generate(TypeScriptEnum typeScriptEnum)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"export enum {typeScriptEnum.Name} {{");

        foreach (var value in typeScriptEnum.EnumValues)
        {
            sb.AppendLine($"    {value} = \"{value}\",");
        }

        sb.AppendLine("}");

        return Task.FromResult(sb.ToString());
    }
}