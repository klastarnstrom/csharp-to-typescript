using System.Diagnostics.CodeAnalysis;

namespace CsharpToTypeScript.Library;

public record TypeMetadata
{
    public required Type Type { get; init; }
    public required string Name { get; init; }
    
    public bool IsClass { get; init; }
    public bool IsInterface { get; init; }
    
    public List<TypeDataMember> DataMembers { get; init; }
    
    public bool IsEnum { get; init; }
    public bool IsNullable { get; init; }

    [MemberNotNullWhen(true, nameof(IsEnum))]
    public List<string>? EnumValues { get; init; }

    public bool IsDictionary { get; init; }

    [MemberNotNullWhen(true, nameof(IsDictionary))]
    public Type? DictionaryKeyType { get; init; }

    [MemberNotNullWhen(true, nameof(IsDictionary))]
    public Type? DictionaryValueType { get; init; }
}

public record TypeDataMember
{
    public string Name { get; init; }
    public bool IsArray { get; init; }
    public TypeMetadata MetaData { get; init; }
}