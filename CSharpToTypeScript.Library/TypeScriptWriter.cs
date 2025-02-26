
using CSharpToTypeScript.Library.Constants;
using CSharpToTypeScript.Library.Exports.Base;
using CSharpToTypeScript.Library.Exports;

namespace CSharpToTypeScript.Library;

public class TypeScriptWriter : IAsyncDisposable
{
    private readonly StreamWriter _writer;
    private readonly TypeScriptConfiguration _configuration;

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

    internal async Task WriteTypeScriptFile(IEnumerable<ITsExport> typeScriptTypes)
    {
        CreateOutputDirectory();
        ClearFile();

        await Comment($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        
        await EmptyLine();

        foreach (var typeScriptType in typeScriptTypes)
        {
            await _writer.WriteLineAsync(typeScriptType.Export());
            
            await EmptyLine();
        }
    }

    private async Task Comment(string comment)
    {
        await _writer.WriteLineAsync($"{SpecialCharacters.SingleLineComment} {comment}");
    }
    
    private async Task EmptyLine()
    {
        await _writer.WriteLineAsync();
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