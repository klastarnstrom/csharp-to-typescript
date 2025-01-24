
using CSharpToTypeScript.LibraryNew.Constants;
using CSharpToTypeScript.LibraryNew.Exports;
using CSharpToTypeScript.LibraryNew.Exports.Base;

namespace CSharpToTypeScript.LibraryNew;

public class TypeScriptWriter : IDisposable
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

    internal void WriteTypeScriptFile(IEnumerable<ITsExport> typeScriptTypes)
    {
        CreateOutputDirectory();
        ClearFile();

        Comment($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        
        EmptyLine();

        foreach (var typeScriptType in typeScriptTypes)
        {
            _writer.WriteLineAsync(typeScriptType.Export());
            
            EmptyLine();
        }
    }

    private void Comment(string comment)
    {
        _writer.WriteLineAsync($"{SpecialCharacters.SingleLineComment} {comment}");
    }
    
    private void EmptyLine()
    {
        _writer.WriteLineAsync();
    }

    private void ClearFile()
    {
        _writer.BaseStream.SetLength(0);
    }

    private void CreateOutputDirectory()
    {
        Directory.CreateDirectory(_configuration.OutputPath);
    }

    public void Dispose()
    {
        _writer.Dispose();
        GC.SuppressFinalize(this);
    }
}