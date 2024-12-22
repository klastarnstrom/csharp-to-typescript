using CsharpToTypeScript.Library.Resolvers;

namespace CsharpToTypeScript.Library.TypeGenerators;

internal class DataMemberGenerator
{
    internal string Generate(DataMemberResolveResult dataMember)
    {
        var firstLetter = dataMember.Name[0];
        var restOfName = dataMember.Name[1..];
        var memberName = char.ToLower(firstLetter) + restOfName;

        var memberString = memberName;
            
        if (dataMember.IsNullable)
        {
            memberString += "?";
        }
            
        memberString += $": {dataMember.Type}";
        
        if (dataMember.IsArray)
        {
            memberString += "[]";
        }
        
        return memberString;
    }
}