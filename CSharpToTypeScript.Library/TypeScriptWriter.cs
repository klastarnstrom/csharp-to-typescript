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
        var randomFileName = Path.GetRandomFileName();
        var filePath = Path.Combine(configuration.OutputPath, $"{randomFileName}.ts");
        _writer = new(filePath);
        _configuration = configuration;
    }

    internal async Task WriteTypeScriptFile(Dictionary<Type, TypeScriptType> typeScriptTypes)
    {
        await using var writer = new TypeScriptWriter(_configuration);

        writer.CreateOutputDirectory();
        writer.ClearFile();

        await writer.Comment($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        foreach (var (_, typeScriptType) in typeScriptTypes)
        {
            if (typeScriptType is TypeScriptEnum tsEnum)
            {
                await writer.AddType(new EnumGenerator().Generate(tsEnum));
            }

            if (typeScriptType is TypeScriptInterface tsInterface)
            {
                await writer.AddLine($"export interface {typeScriptType.Name} {{");

                foreach (var property in tsInterface.Properties)
                {
                    await writer.AddLine($"    {property.Name}: {property.Type.Name};");
                }

                await writer.AddLine("}");
            }
        }
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