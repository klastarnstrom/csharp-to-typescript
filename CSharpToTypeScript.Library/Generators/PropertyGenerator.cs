using System.Text;
using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Models.Properties;

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

        switch (property)
        {
            case TypeScriptArrayProperty arrayProperty:
                builder.Append($"{arrayProperty.ElementType.Name}[]");
                break;
            case TypeScriptDictionaryProperty dictionaryProperty:
                builder.Append($"{{ [key: {dictionaryProperty.KeyType.Name}]: {dictionaryProperty.ValueType.Name} }}");
                break;
            default:
                builder.Append($"{property.Type?.Name}");
                break;
        }

        builder.AppendLine(";");
    }
}