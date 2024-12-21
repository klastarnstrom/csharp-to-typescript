using System.Reflection;

namespace CsharpToTypeScript.Library;

public record DataMemberResolveResult(
    string Name,
    string Type,
    string AccessModifier,
    bool IsNullable,
    string? DefaultValue);

public class DataMemberResolver
{
    public DataMemberResolver Resolve(MemberInfo member)
    {
        var name = member.Name;
        var type = new TypeResolver().Resolve(member.MemberType);

        return new(name, type, accessModifier, isNullable, defaultValue);
    }
}