using System.Text.Json;
using System.Text.RegularExpressions;

namespace CSharpToTypeScript.Library.Models;

public partial record TypeScriptProperty(string Name, TypeScriptType Type, bool IsArray = false)
{
    public string CamelCaseName => ToCamelCase(Name);

    private static string ToCamelCase(string pascalCase)
    {
        // Check if the input is not null or empty
        if (string.IsNullOrEmpty(pascalCase))
        {
            return pascalCase;
        }

        // Define a regular expression for detecting acronyms
        var acronymRegex = PascalCaseRegex();

        // If the first part of the string is an acronym, don't convert it
        if (acronymRegex.IsMatch(pascalCase))
        {
            return pascalCase[0].ToString().ToLower() + pascalCase[1..];
        }

        var numberRegex = DigitRegex();

        // If the string starts with a number, leave it as is and just lowercase the first letter of the rest
        if (numberRegex.IsMatch(pascalCase))
        {
            return pascalCase[0].ToString() + char.ToLower(pascalCase[1]) + pascalCase[2..];
        }

        // Otherwise, convert the first letter to lowercase to make it camelCase
        return char.ToLower(pascalCase[0]) + pascalCase[1..];
    }

    [GeneratedRegex(@"^[A-Z]+(?=[A-Z][a-z])")]
    private static partial Regex PascalCaseRegex();
    
    [GeneratedRegex(@"^\d")]
    private static partial Regex DigitRegex();
}
