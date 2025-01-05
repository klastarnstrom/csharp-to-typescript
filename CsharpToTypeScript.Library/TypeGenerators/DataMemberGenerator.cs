namespace CsharpToTypeScript.Library.TypeGenerators;

internal class DataMemberGenerator
{
    internal string Generate(TypeDataMember dataMember)
    {
        var firstLetter = dataMember.Name[0];
        var restOfName = dataMember.Name[1..];
        var memberName = char.ToLower(firstLetter) + restOfName;

        var memberString = memberName;
            
        if (dataMember.MetaData.IsNullable)
        {
            memberString += "?";
        }
            
        memberString += $": {dataMember.MetaData.Name}";
        
        if (dataMember.IsArray)
        {
            memberString += "[]";
        }
        
        return memberString;
    }
}