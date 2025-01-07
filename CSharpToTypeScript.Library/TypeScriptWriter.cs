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
        var filePath = Path.Combine(configuration.OutputPath, configuration.FileName);
        _writer = new(filePath);
        _configuration = configuration;
    }

    internal async Task WriteTypeScriptFile(Dictionary<Type, TypeScriptType> typeScriptTypes)
    {
        CreateOutputDirectory();
        ClearFile();

        await Comment($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        foreach (var (_, typeScriptType) in typeScriptTypes)
        {
            switch (typeScriptType)
            {
                case TypeScriptEnum tsEnum:
                    await _writer.WriteLineAsync(await EnumGenerator.Generate(tsEnum));
                    break;
                case TypeScriptInterface tsInterface:
                    await _writer.WriteLineAsync(await InterfaceGenerator.Generate(tsInterface));
                    break;
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