using System.Reflection;

namespace CsharpToTypeScript.Library.Resolvers;

public class DataMembersResolver
{
    private readonly FieldResolver _fieldResolver = new();
    private readonly PropertyResolver _propertyResolver = new();
    
    public IEnumerable<DataMemberResolveResult> Resolve(Type type)
    {
        var memberResults = new List<DataMemberResolveResult>();

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var field in fields)
        {
            memberResults.Add(_fieldResolver.Resolve(field));
        }
        
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            memberResults.Add(_propertyResolver.Resolve(property));
        }

        return memberResults;
    }
}