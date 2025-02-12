using CSharpToTypeScript.Library.Exports.Base;

namespace CSharpToTypeScript.Library.Exports.Types;

public class TsEnum : TsExport
{
    private readonly string[] _values;
    private readonly string _typeName;

    public TsEnum(Type type, string typeName) : base(type, typeName)
    {
        _values = Enum.GetNames(type);
        _typeName = typeName;
    }

    public override string Export()
    {
        Builder.AppendLine($"export enum {_typeName} {{");

        for (var i = 0; i < _values.Length; i++)
        {
            Builder.AppendLine($"    {_values[i]} = {i},");
        }

        Builder.AppendLine("}");

        return Builder.ToString();
    }
}