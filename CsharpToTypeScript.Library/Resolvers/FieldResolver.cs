using System.Reflection;

namespace CsharpToTypeScript.Library.Resolvers;

public class FieldResolver : DataMemberResolverBase
{
    private readonly DataTypeResolver _dataTypeResolver = new();

    public DataMemberResolveResult Resolve(FieldInfo member)
    {
        var name = member.Name;
        var type = _dataTypeResolver.Resolve(member.FieldType);
        var isNullable = IsNullable(member.FieldType);

        return new(name, type, isNullable);
    }
}