using System.Text;
using CsharpToTypeScript.Library.Resolvers;

namespace CsharpToTypeScript.Library.TypeGenerators;

internal class EnumGenerator
{
    public string Generate(EnumResolveResult result)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"export enum {result.Name} {{");
        
        foreach (var value in result.Values)
        {
            sb.AppendLine($"    {value} = \"{value}\",");
        }
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }
}