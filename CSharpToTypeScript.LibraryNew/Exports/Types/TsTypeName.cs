using System.Collections.ObjectModel;
using CSharpToTypeScript.LibraryNew.Exports.Types.BuiltIn;
using CSharpToTypeScript.LibraryNew.Extensions;

namespace CSharpToTypeScript.LibraryNew.Exports.Types;

public class TsTypeName(Type type)
{
    private static readonly ReadOnlyDictionary<Type, string> ConvenienceTypes = new(new Dictionary<Type, string>
    {
        { typeof(DateTime), TsDateTimeString.Name },
        { typeof(DateTime?), TsDateTimeString.Name },
        { typeof(DateOnly), TsDateString.Name },
        { typeof(DateOnly?), TsDateString.Name },
        { typeof(Guid), TsGuid.Name }
    });

    private static readonly ReadOnlyDictionary<Type, string> BuiltInReferenceTypes = new(new Dictionary<Type, string>
    {
        { typeof(TimeSpan), "string" },
        { typeof(string), "string" },
    });

    private static readonly ReadOnlyDictionary<Type, string> PrimitiveTypes = new(new Dictionary<Type, string>
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
        { typeof(bool), "boolean" },
        { typeof(bool?), "boolean" },
        { typeof(char), "string" },
        { typeof(char?), "string" },
    });

    public string Name => GetName(type);
    
    public static string GetRootTypeName(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is { } underlyingType)
        {
            return GetRootTypeName(underlyingType);
        }
        
        var systemType = ConvenienceTypes.GetValueOrDefault(type) ??
                         BuiltInReferenceTypes.GetValueOrDefault(type) ??
                         PrimitiveTypes.GetValueOrDefault(type);

        if (systemType is not null)
        {
            return systemType;
        }

        var name = type.Name;

        if (type.IsEnum || !type.IsGenericType)
        {
            return name;
        }

        var genericArguments = type.GetGenericArguments();

        if (type.IsDictionary())
        {
            return $"{{ [key: {GetName(genericArguments[0])}]: {GetName(genericArguments[1])} }}";
        }

        var genericTypeName = name[..name.IndexOf('`')];

        var genericArgumentsNames = genericArguments.Select(GetName);

        return $"{genericTypeName}<{string.Join(", ", genericArgumentsNames)}>";
    }

    private static string GetName(Type type)
    {
        var isEnumerable = type.IsEnumerable();

        var effectiveType = isEnumerable ? type.GetEnumerableElementType() : type;

        var rootTypeName = GetRootTypeName(effectiveType);

        return isEnumerable ? $"{rootTypeName}[]" : rootTypeName;
    }

    private static string GetRootTypeName(Type type)
    {
        var systemType = ConvenienceTypes.GetValueOrDefault(type) ??
                         BuiltInReferenceTypes.GetValueOrDefault(type) ??
                         PrimitiveTypes.GetValueOrDefault(type);

        if (systemType is not null)
        {
            return systemType;
        }

        var name = type.Name;

        if (type.IsEnum || !type.IsGenericType)
        {
            return name;
        }

        var genericArguments = type.GetGenericArguments();

        if (type.IsDictionary())
        {
            return $"{{ [key: {GetName(genericArguments[0])}]: {GetName(genericArguments[1])} }}";
        }

        var genericTypeName = name[..name.IndexOf('`')];

        var genericArgumentsNames = genericArguments.Select(GetName);

        return $"{genericTypeName}<{string.Join(", ", genericArgumentsNames)}>";
    }
}