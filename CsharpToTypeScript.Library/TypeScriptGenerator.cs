using System.Reflection;
using CsharpToTypeScript.Library.Attributes;
using CsharpToTypeScript.Library.Resolvers;
using CsharpToTypeScript.Library.TypeGenerators;

namespace CsharpToTypeScript.Library;

public class TypeScriptGenerator(TypeScriptConfiguration? configuration = null)
{
    private readonly TypeScriptConfiguration _configuration = configuration ?? new();
    private readonly TypesResolver _typesResolver = new();
    private readonly TypeDeclarationGenerator _typeDeclarationGenerator = new();
    private readonly EnumGenerator _enumGenerator = new();

    public async Task Generate()
    {
        await using var writer = new TypeScriptWriter(_configuration);

        writer.CreateOutputDirectory();
        writer.ClearFile();

        await writer.Comment($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        var typesWithAttribute = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => Attribute.IsDefined(type, typeof(TsGenerateAttribute)))
            .ToHashSet();

        var visitedTypes = new Dictionary<Type, TypeMetadata>();

        foreach (var type in typesWithAttribute)
        {
            CollectReferencedTypes(type, visitedTypes);
        }

        foreach (var type in visitedTypes.Keys)
        {
            Console.WriteLine(type.Name);
        }
    }

    private static void CollectReferencedTypes(Type type, Dictionary<Type, TypeMetadata> visited)
    {
        if (IsSystemType(type))
        {
            return;
        }

        if (visited.GetValueOrDefault(type) is not null)
        {
            return;
        }

        if (IsArrayOrEnumerable(type))
        {
            CollectReferencedTypes(GetArrayElementType(type), visited);
        }
        else
        {
            visited.TryAdd(type, new()
            {
                Type = type,
                Name = type.Name,
                IsClass = type.IsClass,
                IsInterface = type.IsInterface,
                IsEnum = type.IsEnum,
                IsNullable = Nullable.GetUnderlyingType(type) != null,
                DataMembers = [],
            });
        }

        if (type.IsGenericType)
        {
            foreach (var genericTypeArgument in type.GetGenericArguments())
            {
                CollectReferencedTypes(genericTypeArgument, visited);
            }
        }

        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        foreach (var fieldInfo in type.GetFields(bindingFlags))
        {
            CollectReferencedTypes(fieldInfo.FieldType, visited);
        }

        foreach (var propertyInfo in type.GetProperties(bindingFlags))
        {
            CollectReferencedTypes(propertyInfo.PropertyType, visited);
        }
    }

    private static bool IsSystemType(Type type) => type.Namespace?.StartsWith("System") == true;

    private static Type GetArrayElementType(Type type)
    {
        // If type implements IEnumerable<> return the generic type argument
        if (type.IsGenericType && IsArrayOrEnumerable(type))
        {
            return type.GetGenericArguments()[0];
        }

        // If type is regular array, return the element type
        return type.GetElementType() ?? throw new ArgumentException("Type is not an array", nameof(type));
    }

    private static bool IsArrayOrEnumerable(Type type)
    {
        if (type.IsArray)
        {
            return true;
        }

        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
               type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }

    private string GenerateTypeString(TypeMetadata metaData)
    {
        if (metaData.IsEnum)
        {
            return _enumGenerator.Generate(metaData);
        }

        return _typeDeclarationGenerator.Generate(metaData);
    }
}