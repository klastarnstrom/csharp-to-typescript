namespace CSharpToTypeScript.Library.Generators;

public static class BuiltInTypesGenerator
{
    public static async Task Generate(StringWriter writer)
    {
        await writer.WriteLineAsync("type Guid = string;");
        await writer.WriteLineAsync("type DateTimeString = string;");
        await writer.WriteLineAsync("type DateString = string;");
    }
}