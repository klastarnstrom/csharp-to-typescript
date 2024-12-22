using System.Text;
using CsharpToTypeScript.Library.Constants;
using CsharpToTypeScript.Library.Resolvers;

namespace CsharpToTypeScript.Library.TypeGenerators;

internal class InterfaceGenerator
{
    private readonly DataMemberGenerator _dataMemberGenerator = new();
    
    public string Generate(InterfaceResolveResult result)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"export interface {result.Name} {{");
        
        foreach (var dataMember in result.DataMembers)
        {
            sb.AppendLine($"{SpecialCharacters.Tab}{_dataMemberGenerator.Generate(dataMember)}");
        }
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }
}