using CSharpToTypeScript.Library.Constants;

namespace CSharpToTypeScript.Library.Generators;

public sealed class TypeScriptStringWriter : StringWriter
{
    public TypeScriptStringWriter()
    {
        NewLine = SpecialCharacters.UnixNewLine;
    }
}