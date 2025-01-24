using System.Reflection;

namespace CSharpToTypeScript.LibraryNew;

public class TypeScriptConfiguration
{
    public required IEnumerable<Assembly> Assemblies { get; init; }
    public string OutputPath { get; set; } = "output";
    public string FileName { get; set; } = "types.ts";
}