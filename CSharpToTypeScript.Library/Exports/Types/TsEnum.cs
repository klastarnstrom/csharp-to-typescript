using CSharpToTypeScript.Library.Exports.Base;

namespace CSharpToTypeScript.Library.Exports.Types;

public class TsEnum(Type type) : TsExport(type)
{
    private string[] Values { get; } = Enum.GetNames(type);
    
    public override string Export()
    {
        Builder.WriteLine($"export enum {TypeName.Name} {{");
        
        foreach (var value in Values)
        {
            Builder.WriteLine($"\t{value} = \"{value}\",");
        }
        
        Builder.WriteLine("}");
        
        return Builder.ToString();
    }
}