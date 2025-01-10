using System.Text;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library.Generators;

public static class PropertyGenerator
{
    public static void Generate(StringBuilder builder, TypeScriptProperty property)
    {
        builder.Append($"    {property.Name}: {property.Type.Name}");

        if (property.IsArray)
        {
            builder.Append("[]");
        }

        builder.AppendLine(";");
    }
}