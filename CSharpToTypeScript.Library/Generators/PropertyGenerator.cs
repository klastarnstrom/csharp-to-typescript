using System.Text;
using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library.Generators;

public static class PropertyGenerator
{
    public static void Generate(StringBuilder builder, TypeScriptProperty property)
    {
        builder.Append($"{SpecialCharacters.Tab}{property.Name}: {property.Type.Name}");

        if (property.IsArray)
        {
            builder.Append("[]");
        }

        builder.AppendLine(";");
    }
}