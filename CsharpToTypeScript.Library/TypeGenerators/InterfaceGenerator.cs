using System.Text;
using CsharpToTypeScript.Library.Resolvers;

namespace CsharpToTypeScript.Library.TypeGenerators;

public class InterfaceGenerator
{
    private readonly DataMembersResolver _dataMemberResolver = new();
    private readonly DataMemberGenerator _dataMemberGenerator = new();
    
    public string Generate(Type type)
    {
        var name = type.Name;
        
        IEnumerable<DataMemberResolveResult> dataMembers = _dataMemberResolver.Resolve(type);
        
        var sb = new StringBuilder();
        
        sb.AppendLine($"export interface {name} {{");
        
        foreach (var dataMember in dataMembers)
        {
            sb.AppendLine($"    {_dataMemberGenerator.Generate(dataMember)}");
        }
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }
}