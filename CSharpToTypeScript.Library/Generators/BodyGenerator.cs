using System.Text;
using CSharpToTypeScript.Library.Models.Properties;

namespace CSharpToTypeScript.Library.Generators;

public static class BodyGenerator
{
    public static void Generate(StringBuilder builder, List<TypeScriptProperty> properties)
    {
        builder.AppendLine(" {");
        
        if (properties.Count == 0)
        {
            builder.Append("}");
        }
        else
        {
            foreach (var property in properties)
            {
                PropertyGenerator.Generate(builder, property);
            }

            builder.AppendLine("}");
        }
    }
}