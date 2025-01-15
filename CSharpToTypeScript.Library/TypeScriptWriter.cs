using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Generators;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Library;

public class TypeScriptWriter : IAsyncDisposable
{
    private readonly TypeScriptConfiguration _configuration;
    private readonly StreamWriter _writer;

    public TypeScriptWriter(TypeScriptConfiguration configuration)
    {
        var outputDirectory = new DirectoryInfo(configuration.OutputPath);

        Directory.CreateDirectory(outputDirectory.FullName);

        var filePath = Path.Combine(configuration.OutputPath, configuration.FileName);

        _writer = new(filePath)
        {
            NewLine = SpecialCharacters.UnixNewLine
        };

        _configuration = configuration;
    }

    internal async Task WriteTypeScriptFile(Dictionary<Type, TypeScriptType> typeScriptTypes)
    {
        CreateOutputDirectory();
        ClearFile();

        await Comment($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        foreach (var (_, typeScriptType) in typeScriptTypes)
        {
            if (typeScriptType is TypeScriptInterface tsInterface
                and ({ IsGeneric: false, IsGenericParameter: false } or { IsOpenGenericType: true }))
            {
                await _writer.WriteLineAsync($"export {await InterfaceGenerator.Generate(tsInterface)}");
            }
            else if (typeScriptType is TypeScriptEnum tsEnum)
            {
                await _writer.WriteLineAsync($"export {await EnumGenerator.Generate(tsEnum)}");
            }
        }
    }

    private async Task Comment(string comment)
    {
        await _writer.WriteLineAsync($"{SpecialCharacters.SingleLineComment} {comment}");
    }

    private void ClearFile()
    {
        _writer.BaseStream.SetLength(0);
    }

    private void CreateOutputDirectory()
    {
        Directory.CreateDirectory(_configuration.OutputPath);
    }

    public async ValueTask DisposeAsync()
    {
        await _writer.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}