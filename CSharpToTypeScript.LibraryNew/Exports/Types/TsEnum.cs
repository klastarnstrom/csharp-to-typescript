using CSharpToTypeScript.LibraryNew.Exports.Base;

namespace CSharpToTypeScript.LibraryNew.Exports.Types;

public class TsEnum(Type type) : TsExport(type)
{
    private string[] Values { get; } = Enum.GetNames(type);
    
    public override string Export()
    {
        Builder.AppendLine($"export enum {TypeName.Name} {{");
        
        for (var i = 0; i < Values.Length; i++)
        {
            Builder.AppendLine($"    {Values[i]} = {i},");
        }
        
        Builder.AppendLine("}");
        
        return Builder.ToString();
    }
}