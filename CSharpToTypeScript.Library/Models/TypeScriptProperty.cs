namespace CSharpToTypeScript.Library.Models;

public record TypeScriptProperty(string Name, TypeScriptType Type, bool IsArray = false)
{
    public string CamelCaseName => ToCamelCase(Name);

    // Convert PascalCase to camelCase
    // Copied from src/libraries/System.Text.Json/Common/JsonCamelCaseNamingPolicy.cs
    private static string ToCamelCase(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !char.IsUpper(propertyName[0]))
        {
            return propertyName;
        }

        return string.Create(propertyName.Length, propertyName, (chars, name) =>
        {
            name.CopyTo(chars);
            FixCasing(chars);
        });
    }

    private static void FixCasing(Span<char> chars)
    {
        for (var i = 0; i < chars.Length; i++)
        {
            if (i == 1 && !char.IsUpper(chars[i]))
            {
                break;
            }

            var hasNext = i + 1 < chars.Length;

            // Stop when next char is already lowercase.
            if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
            {
                // If the next char is a space, lowercase current char before exiting.
                if (chars[i + 1] == ' ')
                {
                    chars[i] = char.ToLowerInvariant(chars[i]);
                }

                break;
            }

            chars[i] = char.ToLowerInvariant(chars[i]);
        }
    }
}