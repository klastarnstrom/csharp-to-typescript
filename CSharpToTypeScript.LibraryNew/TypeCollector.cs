using System.Reflection;
using CSharpToTypeScript.LibraryNew.Attributes;

namespace CSharpToTypeScript.LibraryNew;

public class TypeCollector
{
    public static List<Type> CollectTypes(IEnumerable<Assembly> assemblies)
    {
        var typesWithAttribute = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            // IsPublic does not cover nested types
            .Where(type => !type.IsNotPublic && Attribute.IsDefined(type, typeof(TsGenerateAttribute)))
            .ToHashSet();

        List<Type> collectedTypes = [];

        foreach (var type in typesWithAttribute)
        {
            if (!collectedTypes.Contains(type))
            {
                collectedTypes.Add(type);
            }
        }
        
        return collectedTypes;
    }
}