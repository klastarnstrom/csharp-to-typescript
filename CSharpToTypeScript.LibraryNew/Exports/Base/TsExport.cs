using System.Text;
using CSharpToTypeScript.LibraryNew.Exports.Types;

namespace CSharpToTypeScript.LibraryNew.Exports.Base;

public interface ITsExport
{
    public string Export();
}

public abstract class TsExport(Type type) : ITsExport
{
    public Type Type { get; } = type;
    protected StringBuilder Builder { get; } = new();
    protected TsTypeName TypeName { get; } = new(type);
    public abstract string Export();
}