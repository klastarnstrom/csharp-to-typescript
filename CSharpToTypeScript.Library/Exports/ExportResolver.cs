using System.Reflection;
using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Exports.Types;
using CSharpToTypeScript.Library.Extensions;

namespace CSharpToTypeScript.Library.Exports;

public class ExportResolver
{
    public ExportContext ExportContext { get; } = new();

    public void ResolveType(Type type)
    {
        if (SkipType(type))
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
                // Generic argument can't be safely ignored
                ThrowIfTypeIsIgnored(genericArgument, ErrorMessage.GenericArgument(genericArgument));
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

            // Property should be ignored if its type is ignored
            ThrowIfTypeIsIgnored(property.PropertyType, ErrorMessage.PropertyTypeIsIgnored(property.PropertyType));

            ResolveType(property.PropertyType);

            tsInterface.AddProperty(property);
        }

        if (type.BaseType != null && !SkipType(type.BaseType))
        {
            tsInterface.SetBaseType(type.BaseType);
            ResolveType(type.BaseType);
        }

        var implementedInterfaces = type.GetInterfaces()
            .Where(t => !SkipType(t));

        foreach (var interfaceType in implementedInterfaces)
        {
            tsInterface.AddImplementedInterface(interfaceType);
            ResolveType(interfaceType);
        }

        return tsInterface;
    }

    private static bool SkipType(Type type)
    {
        if (type.IsIgnored())
        {
            return true;
        }

        if (type.IsSystemType())
        {
            return true;
        }

        if (type == typeof(object))
        {
            return true;
        }
        
        return false;
    }

    private static void ThrowIfTypeIsIgnored(Type type, string message)
    {
        if (type.IsIgnored())
        {
            throw new InvalidOperationException(message);
        }
    }
}