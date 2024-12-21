using CsharpToTypeScript.Library.Constants;
using CsharpToTypeScript.Library.Resolvers.Base;

namespace CsharpToTypeScript.Library;

internal abstract record TypeResolveResult(string Name, string FullName, GenerateType GenerateType);

internal record InterfaceResolveResult(string Name, string FullName, IEnumerable<DataMemberResolveResult> DataMembers)
    : TypeResolveResult(Name, FullName, GenerateType.Interface);

internal record EnumResolveResult(string Name, string FullName, IEnumerable<string> Values) : TypeResolveResult(Name, FullName, GenerateType.Enum);