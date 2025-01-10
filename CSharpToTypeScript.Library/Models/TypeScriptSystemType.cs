using System.Collections.ObjectModel;

namespace CSharpToTypeScript.Library.Models;

public class TypeScriptSystemType(string name) : TypeScriptType(name)
{
    public string TypeName { get; } = name;

    private static readonly ReadOnlyDictionary<Type, string> TypeMap = new(new Dictionary<Type, string>
    {
        { typeof(DateTime), "Date" },
        { typeof(Guid), "string" },
        { typeof(TimeSpan), "string" },
        { typeof(bool), "boolean" },
        { typeof(bool?), "boolean" },
        { typeof(char), "string" },
        { typeof(char?), "string" },
        { typeof(string), "string" },
    });

    public static readonly ReadOnlyDictionary<Type, string> NumberTypeMap = new(new Dictionary<Type, string>
    {
        { typeof(decimal), "number" },
        { typeof(decimal?), "number" },
        { typeof(double), "number" },
        { typeof(double?), "number" },
        { typeof(float), "number" },
        { typeof(float?), "number" },
        { typeof(int), "number" },
        { typeof(int?), "number" },
        { typeof(long), "number" },
        { typeof(long?), "number" },
        { typeof(sbyte), "number" },
        { typeof(sbyte?), "number" },
        { typeof(short), "number" },
        { typeof(short?), "number" },
        { typeof(uint), "number" },
        { typeof(uint?), "number" },
        { typeof(ulong), "number" },
        { typeof(ulong?), "number" },
        { typeof(ushort), "number" },
        { typeof(ushort?), "number" },
        { typeof(byte), "number" },
        { typeof(byte?), "number" },
    });

    public static TypeScriptType Create(Type type) =>
        new TypeScriptSystemType(NumberTypeMap.GetValueOrDefault(type) ?? TypeMap.GetValueOrDefault(type) ?? "any");
}