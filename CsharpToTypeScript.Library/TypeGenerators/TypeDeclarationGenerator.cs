using System.Text;
using CsharpToTypeScript.Library.Constants;
using CsharpToTypeScript.Library.Resolvers;

namespace CsharpToTypeScript.Library.TypeGenerators;

internal class TypeDeclarationGenerator
{
    private readonly DataMemberGenerator _dataMemberGenerator = new();
    
    public string Generate(TypeMetadata metaData)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"export interface {metaData.Name} {{");
        
        foreach (var dataMember in metaData.DataMembers)
        {
            sb.AppendLine($"{SpecialCharacters.Tab}{_dataMemberGenerator.Generate(dataMember)}");
        }
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }
}