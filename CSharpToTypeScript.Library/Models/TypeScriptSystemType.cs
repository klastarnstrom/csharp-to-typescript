using System.Collections.ObjectModel;

namespace CSharpToTypeScript.Library.Models;

public class TypeScriptSystemType(string name) : TypeScriptType(name)
{
    public string TypeName { get; } = name;

    // Type to TypeScript type name mapping
    private static readonly ReadOnlyDictionary<Type, string> TypeMap = new(new Dictionary<Type, string>
    {
        { typeof(string), "string" },
        { typeof(int), "number" },
        { typeof(long), "number" },
        { typeof(short), "number" },
        { typeof(byte), "number" },
        { typeof(float), "number" },
        { typeof(double), "number" },
        { typeof(decimal), "number" },
        { typeof(bool), "boolean" },
        { typeof(DateTime), "Date" },
        { typeof(Guid), "string" },
        { typeof(TimeSpan), "string" },
        { typeof(char), "string" },
        { typeof(sbyte), "number" },
        { typeof(uint), "number" },
        { typeof(ulong), "number" },
        { typeof(ushort), "number" },
        { typeof(byte?), "number" },
        { typeof(sbyte?), "number" },
        { typeof(short?), "number" },
        { typeof(ushort?), "number" },
        { typeof(int?), "number" },
        { typeof(uint?), "number" },
        { typeof(long?), "number" },
        { typeof(ulong?), "number" },
        { typeof(float?), "number" },
        { typeof(double?), "number" },
        { typeof(decimal?), "number" },
        { typeof(bool?), "boolean" },
        { typeof(char?), "string" },
    });

    public static TypeScriptType Create(Type type) =>
        new TypeScriptSystemType(TypeMap.GetValueOrDefault(type) ?? "any");
}