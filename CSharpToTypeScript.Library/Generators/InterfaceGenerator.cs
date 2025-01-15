using System.Text;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library.Generators;

public static class InterfaceGenerator
{
    public static async Task<string> Generate(TypeScriptInterface tsInterface)
    {
        var writer = new TypeScriptStringWriter();
        
        await writer.WriteAsync("interface ");

        await TypeNameGenerator.Generate(writer, tsInterface);

        var hasBaseType = tsInterface.BaseType != null;
        var hasImplementedInterfaces = tsInterface.ImplementedInterfaces.Count != 0;
        var hasInheritance = hasBaseType || hasImplementedInterfaces;

        
        if (hasInheritance)
        {
            await writer.WriteAsync(" extends ");
        }
        
        if (hasBaseType)
        {
            if (tsInterface.BaseType is not TypeScriptInterface baseType)
            {
                throw new InvalidOperationException("Base type is not a TypeScriptInterface");
            }
            
            await TypeNameGenerator.Generate(writer, baseType);
        }
        
        if (hasBaseType && hasImplementedInterfaces)
        {
            await writer.WriteAsync(", ");
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
                
                await TypeNameGenerator.Generate(writer, implementedInterface);

                if (i != tsInterface.ImplementedInterfaces.Count - 1)
                {
                    await writer.WriteAsync(", ");
                }
            }
        }
        
        await BodyGenerator.Generate(writer, tsInterface.Properties);

        return writer.ToString();
    }


}