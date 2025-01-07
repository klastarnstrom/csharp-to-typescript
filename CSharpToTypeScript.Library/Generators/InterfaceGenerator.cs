using System.Text;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library.Generators;

public class InterfaceGenerator
{
    public static Task<string> Generate(TypeScriptInterface tsInterface)
    {
        var builder = new StringBuilder();

        builder.Append($"export interface {tsInterface.Name}");

        var hasBaseType = tsInterface.BaseType != null;
        var hasImplementedInterfaces = tsInterface.ImplementedInterfaces.Count != 0;
        var hasInheritance = hasBaseType || hasImplementedInterfaces;
        
        if (hasInheritance)
        {
            builder.Append(" extends ");
        }
        
        if (hasBaseType)
        {
            builder.Append($"{tsInterface.BaseType!.Name}");
        }
        
        if (hasBaseType && hasImplementedInterfaces)
        {
            builder.Append(", ");
        }

        if (hasImplementedInterfaces)
        {
            for (var i = 0; i < tsInterface.ImplementedInterfaces.Count; i++)
            {
                builder.Append(tsInterface.ImplementedInterfaces[i].Name);

                if (i != tsInterface.ImplementedInterfaces.Count - 1)
                {
                    builder.Append(", ");
                }
            }
        }

        builder.AppendLine(" {");

        foreach (var property in tsInterface.Properties)
        {
            builder.Append($"    {property.CamelCaseName}: {property.Type.Name}");

            if (property.IsArray)
            {
                builder.Append("[]");
            }

            builder.AppendLine(";");
        }

        builder.AppendLine("}");
        
        return Task.FromResult(builder.ToString());
    }
}