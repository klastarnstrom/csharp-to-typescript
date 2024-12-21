using System.Reflection;
using CsharpToTypeScript.Library.Resolvers.Base;

namespace CsharpToTypeScript.Library.Resolvers;

internal class DataMemberResolver : DataMemberResolverBase
{
    private readonly SystemTypeResolver _systemTypeResolver = new();

    internal DataMemberResolveResult Resolve(MemberInfo member)
    {
        var typeInfo = member switch {
            FieldInfo field => field.FieldType,
            PropertyInfo property => property.PropertyType,
            _ => throw new ArgumentException("Member is not a field or property", nameof(member))
        };
        
        var name = member.Name;
        var typeName = _systemTypeResolver.Resolve(typeInfo);
        var isNullable = IsNullable(typeInfo);
        var isArray = IsArray(typeInfo);

        return new(name, typeName, isNullable, isArray);
    }
}