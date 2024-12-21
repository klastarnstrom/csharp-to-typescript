using System.Text;
using CsharpToTypeScript.Library.TypeGenerators.Base;

namespace CsharpToTypeScript.Library.TypeGenerators;

public class EnumGenerator : ITypeDeclarationGenerator
{
    public string Generate(Type type)
    {
        var values = Enum.GetNames(type);
        
        var sb = new StringBuilder();
        
        sb.AppendLine($"export enum {type.Name} {{");
        
        foreach (var value in values)
        {
            sb.AppendLine($"    {value} = \"{value}\",");
        }
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }
}