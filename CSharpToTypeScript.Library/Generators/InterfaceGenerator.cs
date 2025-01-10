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
        var hasGenericArguments = tsInterface.GenericArguments.Count != 0;
        
        if (hasGenericArguments)
        {
            builder.Append(GenerateGenericArguments(tsInterface));
        }
        
        if (hasInheritance)
        {
            builder.Append(" extends ");
        }
        
        if (hasBaseType)
        {
            var baseType = tsInterface.BaseType as TypeScriptInterface;
            
            if (baseType is null)
            {
                throw new InvalidOperationException("Base type is not a TypeScriptInterface");
            }
            
            builder.Append($"{baseType.Name}");
            
            if (baseType.GenericArguments.Count != 0)
            {
                builder.Append(GenerateGenericArguments(baseType));
            }
        }
        
        if (hasBaseType && hasImplementedInterfaces)
        {
            builder.Append(", ");
        }

        if (hasImplementedInterfaces)
        {
            for (var i = 0; i < tsInterface.ImplementedInterfaces.Count; i++)
            {
                var implementedInterface = tsInterface.ImplementedInterfaces[i] as TypeScriptInterface;
                
                if (implementedInterface is null)
                {
                    throw new InvalidOperationException("Implemented interface is not a TypeScriptInterface");
                }
                
                builder.Append(implementedInterface.Name);
                
                if (implementedInterface.GenericArguments.Count != 0)
                {
                    builder.Append(GenerateGenericArguments(implementedInterface));
                }

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

        builder.Append(">");

        return builder.ToString();
    }
}