using System.Reflection;

namespace CSharpToTypeScript.LibraryNew.Exports;

public class TsInterface : TsExport
{
    public override string ExportType => "interface";
    public List<TsProperty> Properties { get; } = [];

    public TsInterface(Type type, ExportResolver exportResolver) : base(type, exportResolver)
    {
        var properties = type
            .GetProperties()
            .Cast<MemberInfo>()
            .Concat(type.GetFields());
        
        foreach (var propertyInfo in properties)
        {
            var property = new TsProperty(propertyInfo);
            
            exportResolver.ResolveType(property.PropertyType);
            
            Properties.Add(property);
        }

        if (type.IsGenericType)
        {
            foreach (var genericArgument in type.GetGenericArguments())
            {
                exportResolver.ResolveType(genericArgument);
            }
        }
    }
}