using System.Text;

namespace CsharpToTypeScript.Library;

public class InterfaceGenerator
{
    public string Generate(string name, IEnumerable<DataMemberResolveResult> dataMembers)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"export interface {name} {{");
        
        foreach (var dataMember in dataMembers)
        {
            var type = dataMember.IsNullable ? $"{dataMember.Type} | null" : dataMember.Type;
            var defaultValue = dataMember.DefaultValue == null ? "" : $" = {dataMember.DefaultValue}";
            
            sb.AppendLine($"    {dataMember.Name}: {type}{defaultValue};");
        }
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }
}