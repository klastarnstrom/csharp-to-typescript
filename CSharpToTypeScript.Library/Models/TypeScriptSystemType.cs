using System.Collections.ObjectModel;

namespace CSharpToTypeScript.Library.Models;

public class TypeScriptSystemType : TypeScriptType
{
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
        { typeof(object), "any" },
    });

    public static TypeScriptType Create(Type type)
    {
        return new TypeScriptSystemType
        {
            Name = TypeMap[type],
        };
    }
}