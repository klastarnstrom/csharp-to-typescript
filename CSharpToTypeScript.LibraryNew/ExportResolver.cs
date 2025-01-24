using CSharpToTypeScript.LibraryNew.Exports;
using CSharpToTypeScript.LibraryNew.Extensions;

namespace CSharpToTypeScript.LibraryNew;

public class ExportResolver
{
    public ExportContext ExportContext { get; } = new();

    public void ResolveType(Type type)
    {
        if (type.FullName?.StartsWith("System.") == true)
        {
            return;
        }
        
        if (type.IsEnum)
        {
            ExportContext.AddExport(new TsEnum(type, this));
        }

        if (type.IsEnumerable())
        {
            var elementType = type.GetEnumerableElementType();
            
            ResolveType(elementType);
        }

        if (type.IsInterface || type.IsClass)
        {
            var export = ResolveInterface(type);

            if (export is not null)
            {
                ExportContext.AddExport(export);
            }
        }
    }

    private TsInterface? ResolveInterface(Type type)
    {
        if (type.IsGenericTypeParameter)
        {
            return null;
        }
        
        return new(type, this);
    }
}