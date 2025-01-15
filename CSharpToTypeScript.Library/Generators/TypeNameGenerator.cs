using System.Text;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library.Generators;

public static class TypeNameGenerator
{
    public static async Task Generate(StringWriter writer, TypeScriptType type)
    {
        await writer.WriteAsync(type.Name);
        
        if (type is TypeScriptInterface tsInterface && tsInterface.GenericArguments.Count != 0)
        {
            await writer.WriteAsync(GenerateGenericArguments(tsInterface));
        }
    }
    
    private static string GenerateGenericArguments(TypeScriptInterface tsInterface)
    {
        if (tsInterface.GenericArguments.Count == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder("<");

        for (var i = 0; i < tsInterface.GenericArguments.Count; i++)
        {
            builder.Append(tsInterface.GenericArguments[i].Name);

            if (i != tsInterface.GenericArguments.Count - 1)
            {
                builder.Append(", ");
            }
        }

        builder.Append('>');

        return builder.ToString();
    }
}