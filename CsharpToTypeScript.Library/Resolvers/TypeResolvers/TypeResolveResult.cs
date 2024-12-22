using CsharpToTypeScript.Library.Constants;

namespace CsharpToTypeScript.Library.Resolvers;

internal abstract record TypeResolveResult(string Name, GenerateType GenerateType);

internal record InterfaceResolveResult(string Name, IEnumerable<DataMemberResolveResult> DataMembers)
    : TypeResolveResult(Name, GenerateType.Interface);

internal record EnumResolveResult(string Name, IEnumerable<string> Values) : TypeResolveResult(Name, GenerateType.Enum);