using CSharpToTypeScript.LibraryNew.Exports.Base;

namespace CSharpToTypeScript.LibraryNew.Exports.Types;

public class TsInterface(Type type) : TsExport(type)
{
    private List<TsProperty> Properties { get; } = [];
    private TsTypeName? BaseType { get; set; }
    private List<TsTypeName> ImplementedInterfaces { get; } = [];

    public override string Export()
    {
        Builder.Append($"export interface {TypeName.Name} ");

        AppendInheritance();

        Builder.AppendLine("{");

        foreach (var property in Properties)
        {
            var nullability = property.IsMarkedAsNullable ? "?" : "";
            
            Builder.AppendLine($"    {property.Name}{nullability}: {property.Type};");
        }

        Builder.AppendLine("}");

        return Builder.ToString();
    }

    public void AddProperty(TsProperty property)
    {
        Properties.Add(property);
    }

    public void AddImplementedInterface(Type implementedInterface)
    {
        ImplementedInterfaces.Add(new(implementedInterface));
    }

    public void SetBaseType(Type baseType)
    {
        BaseType = new(baseType);
    }

    private void AppendInheritance()
    {
        if (BaseType is null && ImplementedInterfaces.Count == 0)
        {
            return;
        }

        Builder.Append("extends ");

        if (BaseType is not null)
        {
            Builder.Append(new TsTypeName(Type.BaseType!).Name);

            if (ImplementedInterfaces.Count > 0)
            {
                Builder.Append(", ");
            }
        }

        for (var i = 0; i < ImplementedInterfaces.Count; i++)
        {
            Builder.Append(ImplementedInterfaces[i].Name);

            if (i < ImplementedInterfaces.Count - 1)
            {
                Builder.Append(", ");
            }
        }
    }
}