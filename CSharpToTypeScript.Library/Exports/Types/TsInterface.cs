using CSharpToTypeScript.Library.Exports.Base;

namespace CSharpToTypeScript.Library.Exports.Types;

public class TsInterface(Type type) : TsExport(type)
{
    private List<TsProperty> Properties { get; } = [];
    private TsTypeName? BaseType { get; set; }
    private List<TsTypeName> ImplementedInterfaces { get; } = [];

    public override string Export()
    {
        Builder.Write($"export interface {TypeName.Name} ");

        WriteInheritance();

        Builder.WriteLine("{");

        foreach (var property in Properties)
        {
            Builder.WriteLine(property.Export());
        }

        Builder.WriteLine("}");

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
        if (Type.IsInterface)
        {
            throw new InvalidOperationException("Interfaces cannot have a base type");
        }

        if (baseType.IsInterface)
        {
            throw new InvalidOperationException("Interfaces cannot be used as a base type");
        }
        
        if (baseType.IsEnum)
        {
            throw new InvalidOperationException("Enums cannot be used as a base type");
        }
        
        BaseType = new(baseType);
    }

    private void WriteInheritance()
    {
        if (BaseType is null && ImplementedInterfaces.Count == 0)
        {
            return;
        }
        
        Builder.Write("extends ");

        if (BaseType is not null)
        {
            if (Type.BaseType is null)
            {
                throw new InvalidOperationException("Base type is null");
            }
            
            Builder.Write(BaseType.Name);

            if (ImplementedInterfaces.Count > 0)
            {
                Builder.Write(", ");
            }
        }

        for (var i = 0; i < ImplementedInterfaces.Count; i++)
        {
            Builder.Write(ImplementedInterfaces[i].Name);

            if (i < ImplementedInterfaces.Count - 1)
            {
                Builder.Write(", ");
            }
        }
        
        Builder.Write(" ");
    }
}