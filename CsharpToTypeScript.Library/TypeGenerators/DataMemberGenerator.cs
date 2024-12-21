using CsharpToTypeScript.Library.Resolvers;

namespace CsharpToTypeScript.Library.TypeGenerators;

public class DataMemberGenerator
{
    public string Generate(DataMemberResolveResult dataMember)
    {
        var firstLetter = dataMember.Name[0];
        var restOfName = dataMember.Name[1..];
        var memberName = char.ToLower(firstLetter) + restOfName;

        var memberString = memberName;
            
        if (dataMember.IsNullable)
        {
            memberString += "?";
        }
            
        memberString += $": {dataMember.Type};";
        
        return memberString;
    }
}