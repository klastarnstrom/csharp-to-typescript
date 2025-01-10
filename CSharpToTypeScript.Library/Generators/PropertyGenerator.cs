using System.Text;
using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library.Generators;

public static class PropertyGenerator
{
    public static void Generate(StringBuilder builder, TypeScriptProperty property)
    {
        builder.Append($"{SpecialCharacters.Tab}{property.Name}");

        if (property.IsNullable)
        {
            builder.Append('?');
        }

        builder.Append(": ");

        if (property is TypeScriptArrayProperty arrayProperty)
        {
            builder.Append($"{arrayProperty.ElementType.Name}[]");
        }
        else
        {
            builder.Append($"{property.Type?.Name}");
        }

        builder.AppendLine(";");
    }
}