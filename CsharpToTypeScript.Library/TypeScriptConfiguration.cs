namespace CsharpToTypeScript.Library;

public class TypeScriptConfiguration
{
    public string OutputPath { get; set; } = "output";
    public string FileName { get; set; } = "types.ts";
    public List<string> IncludedNamespaces { get; set; } = [];
}