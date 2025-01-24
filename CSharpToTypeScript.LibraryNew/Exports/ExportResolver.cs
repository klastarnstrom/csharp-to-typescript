using System.Reflection;
using CSharpToTypeScript.LibraryNew.Exports.Types;
using CSharpToTypeScript.LibraryNew.Extensions;

namespace CSharpToTypeScript.LibraryNew.Exports;

public class ExportResolver
{
    public ExportContext ExportContext { get; } = new();

    public void ResolveType(Type type)
    {
        if (type.IsIgnored())
        {
            return;
        }

        if (type.IsSystemType())
        {
            return;
        }

        if (type.IsEnum)
        {
            ExportContext.AddExport(new TsEnum(type));
        }

        if (type.IsEnumerable())
        {
            ResolveType(type.GetEnumerableElementType());
        }
        else if (type.IsInterface || type.IsClass)
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

        if (type.IsGenericType)
        {
            var typeDefinition = type.GetGenericTypeDefinition();

            if (typeDefinition != type && typeDefinition.ContainsGenericParameters)
            {
                ResolveType(typeDefinition);
            }

            foreach (var genericArgument in type.GetGenericArguments())
            {
                ResolveType(genericArgument);
            }

            // Type is closed generic type
            if (!type.IsGenericTypeDefinition)
            {
                return null;
            }
        }

        var tsInterface = new TsInterface(type);

        var memberInfos = type
            .GetProperties()
            .Cast<MemberInfo>()
            .Concat(type.GetFields())
            .Where(t => t.DeclaringType == type && !t.IsIgnored());

        foreach (var memberInfo in memberInfos)
        {
            var property = new TsProperty(memberInfo);

            ResolveType(property.PropertyType);

            tsInterface.AddProperty(property);
        }

        if (type.BaseType != null && !type.BaseType.IsIgnored() && type.BaseType != typeof(object))
        {
            tsInterface.SetBaseType(type.BaseType);
            ResolveType(type.BaseType);
        }

        var implementedInterfaces = type.GetInterfaces()
            .Where(t => !t.IsIgnored() && !t.IsSystemType());

        foreach (var interfaceType in implementedInterfaces)
        {
            tsInterface.AddImplementedInterface(interfaceType);
            ResolveType(interfaceType);
        }

        return tsInterface;
    }
}