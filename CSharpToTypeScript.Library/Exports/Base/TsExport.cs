using System.Text;
using CSharpToTypeScript.Library.Exports.Types;

namespace CSharpToTypeScript.Library.Exports.Base;

public interface ITsExport
{
    public string Export();
}

public abstract class TsExport : ITsExport
{
    public Type Type { get; }
    protected StringBuilder Builder { get; } = new();
    protected string TypeName { get; }

    protected TsExport(Type type, string typeName)
    {
        Type = type;
        TypeName = typeName;
    }

    public abstract string Export();
}