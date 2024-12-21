using System.Reflection;

namespace CsharpToTypeScript.Library.Resolvers;

public class PropertyResolver : DataMemberResolverBase
{
    private readonly DataTypeResolver _dataTypeResolver = new();

    public DataMemberResolveResult Resolve(PropertyInfo member)
    {
        var name = member.Name;
        var type = _dataTypeResolver.Resolve(member.PropertyType);
        var isNullable = IsNullable(member.PropertyType);

        return new(name, type, isNullable);
    }
}