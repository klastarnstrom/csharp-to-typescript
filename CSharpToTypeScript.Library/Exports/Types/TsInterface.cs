using CSharpToTypeScript.Library.Exports.Base;

namespace CSharpToTypeScript.Library.Exports.Types;

public class TsInterface : TsExport
{
    private readonly List<TsProperty> _properties = [];
    private string? _baseTypeName;
    private readonly List<string> _implementedInterfaceNames = [];
    private readonly string _typeName;
    private readonly TypeNameRegistry _typeNameRegistry;

    public TsInterface(Type type, string typeName, TypeNameRegistry typeNameRegistry) : base(type, typeName)
    {
        _typeName = typeName;
        _typeNameRegistry = typeNameRegistry;
    }

    public override string Export()
    {
        Builder.Append($"export interface {_typeName} ");

        AppendInheritance();

        Builder.AppendLine("{");

        foreach (var property in _properties)
        {
            var nullability = property.IsMarkedAsNullable ? "?" : "";
            var propertyTypeName = _typeNameRegistry.GetTypeName(property.PropertyType);

            Builder.AppendLine($"    {property.Name}{nullability}: {propertyTypeName};");
        }

        Builder.AppendLine("}");

        return Builder.ToString();
    }

    public void AddProperty(TsProperty property)
    {
        _properties.Add(property);
    }

    public void AddImplementedInterface(Type implementedInterface)
    {
        _implementedInterfaceNames.Add(_typeNameRegistry.GetTypeName(implementedInterface));
    }

    public void SetBaseType(Type baseType)
    {
        _baseTypeName = _typeNameRegistry.GetTypeName(baseType);
    }

    private void AppendInheritance()
    {
        if (_baseTypeName is null && _implementedInterfaceNames.Count == 0)
        {
            return;
        }

        Builder.Append("extends ");

        if (_baseTypeName is not null)
        {
            Builder.Append(_baseTypeName);

            if (_implementedInterfaceNames.Count > 0)
            {
                Builder.Append(", ");
            }
        }

        for (var i = 0; i < _implementedInterfaceNames.Count; i++)
        {
            Builder.Append(_implementedInterfaceNames[i]);

            if (i < _implementedInterfaceNames.Count - 1)
            {
                Builder.Append(", ");
            }
        }
    }
}