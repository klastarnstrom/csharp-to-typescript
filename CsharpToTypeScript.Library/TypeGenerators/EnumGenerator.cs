using System.Text;
using CsharpToTypeScript.Library.Resolvers;

namespace CsharpToTypeScript.Library.TypeGenerators;

internal class EnumGenerator
{
    public string Generate(TypeMetadata metaData)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"export enum {metaData.Name} {{");
        
        foreach (var value in metaData.EnumValues ?? [])
        {
            sb.AppendLine($"    {value} = \"{value}\",");
        }
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }
}