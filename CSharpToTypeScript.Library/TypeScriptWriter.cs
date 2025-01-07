using System.Text.Json;
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
        var filePath = Path.Combine(configuration.OutputPath,
            $"{DateTime.Now:yyyy_MM_dd_HH_mm_ss}_{randomFileName}.ts");
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
                await writer.Add($"export interface {typeScriptType.Name}");

                if (tsInterface.BaseType != null)
                {
                    await writer.Add($" extends {tsInterface.BaseType.Name}");
                }

                if (tsInterface.ImplementedInterfaces.Count != 0)
                {
                    await writer.Add($", {string.Join(", ", tsInterface.ImplementedInterfaces.Select(i => i.Name))}");
                }

                await writer.AddLine(" {");

                foreach (var property in tsInterface.Properties)
                {
                    await writer.Add($"    {property.CamelCaseName}: {property.Type.Name}");
                    
                    if (property.IsArray)
                    {
                        await writer.Add("[]");
                    }
                    
                    await writer.AddLine(";");
                }

                await writer.AddLine("}");
            }
        }
    }

    private async Task Add(string text)
    {
        await _writer.WriteAsync(text);
    }

    private async Task AddLine(string? line = "")
    {
        await _writer.WriteLineAsync(line);
    }

    private async Task AddType(string type)
    {
        await AddLine();
        await _writer.WriteLineAsync(type);
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
    }
}