using System.Text;
using CsharpToTypeScript.Library.Constants;

namespace CsharpToTypeScript.Library;

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
    
    public async Task AddLine(string? line = "")
    {
        await _writer.WriteLineAsync(line);
    }
    
    public async Task AddType(string type)
    {
        await AddLine();
        await _writer.WriteLineAsync(type);
    }
    
    public async Task Comment(string comment)
    {
        await _writer.WriteLineAsync($"{SpecialCharacters.SingleLineComment} {comment}");
    }
    
    public void ClearFile()
    {
        _writer.BaseStream.SetLength(0);
    }
    
    public void CreateOutputDirectory()
    {
        Directory.CreateDirectory(_configuration.OutputPath);
    }

    public async ValueTask DisposeAsync()
    {
        await _writer.DisposeAsync();
    }
}