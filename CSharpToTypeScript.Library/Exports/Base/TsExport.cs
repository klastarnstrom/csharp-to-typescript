using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Exports.Types;

namespace CSharpToTypeScript.Library.Exports.Base;

public interface ITsExport
{
    public string Export();
}

public abstract class TsExport(Type type) : ITsExport
{
    public Type Type { get; } = type;
    protected StringWriter Builder { get; } = new() { NewLine = SpecialCharacters.UnixNewLine };
    protected TsTypeName TypeName { get; } = new(type);
    public abstract string Export();
}