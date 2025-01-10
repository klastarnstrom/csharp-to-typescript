namespace CSharpToTypeScript.Library.Models.Properties;

public class TypeScriptProperty
{
    private readonly string _name;
    public string Name => ToCamelCase(_name);
    public TypeScriptType? Type { get; }
    public bool IsNullable { get; }
    
    protected TypeScriptProperty(string name, bool isNullable)
    {
        _name = name;
        IsNullable = isNullable;
    }

    public TypeScriptProperty(string name, TypeScriptType typeScriptType, bool isNullable) : this(name, isNullable)
    {
        Type = typeScriptType;
    }
    
    // Convert PascalCase to camelCase
    // Copied from src/libraries/System.Text.Json/Common/JsonCamelCaseNamingPolicy.cs
    private static string ToCamelCase(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !char.IsUpper(propertyName[0]))
        {
            return propertyName;
        }

        return FixCasing(propertyName.ToCharArray());
    }

    private static string FixCasing(char[] chars)
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
        
        return new(chars);
    }
}