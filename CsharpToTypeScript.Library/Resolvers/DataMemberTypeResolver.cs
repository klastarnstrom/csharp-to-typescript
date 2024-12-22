using System.Collections.ObjectModel;

namespace CsharpToTypeScript.Library.Resolvers;

internal class DataMemberTypeResolver
{
    private readonly ReadOnlyDictionary<Type, string> _typeNames = new(new Dictionary<Type, string>
    {
        { typeof(bool), "boolean" },
        { typeof(bool?), "boolean" },
        { typeof(string), "string" },
        { typeof(char), "string" },
        { typeof(char?), "string" },
        { typeof(DateTime), "Date" },
        { typeof(DateTime?), "Date" },
        { typeof(Guid), "string" },
        { typeof(Guid?), "string" },
    });

    private readonly IReadOnlyCollection<Type> _numericTypes = new List<Type>
    {
        typeof(byte),
        typeof(byte?),
        typeof(sbyte),
        typeof(sbyte?),
        typeof(short),
        typeof(short?),
        typeof(ushort),
        typeof(ushort?),
        typeof(int),
        typeof(int?),
        typeof(uint),
        typeof(uint?),
        typeof(long),
        typeof(long?),
        typeof(ulong),
        typeof(ulong?),
        typeof(float),
        typeof(float?),
        typeof(double),
        typeof(double?),
        typeof(decimal),
        typeof(decimal?)
    };
    
    internal string Resolve(Type type)
    {
        if (_numericTypes.Contains(type))
        {
            return "number";
        }
        
        var typeName = _typeNames.GetValueOrDefault(type);
        
        return typeName ?? type.Name;
    }
}