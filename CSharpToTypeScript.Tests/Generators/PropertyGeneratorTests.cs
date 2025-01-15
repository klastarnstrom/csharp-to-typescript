using CSharpToTypeScript.Library.Generators;
using CSharpToTypeScript.Library.Models;
using CSharpToTypeScript.Library.Models.Properties;

namespace CSharpToTypeScript.Tests.Generators;

[TestFixture]
public class PropertyGeneratorTests
{
    [Test]
    public async Task Generate_Property_Is_Nullable()
    {
        // Arrange
        var property = new TypeScriptProperty("Name", new TypeScriptSystemType("string"), true);
        var writer = new TypeScriptStringWriter();

        // Act
        await PropertyGenerator.Generate(writer, property);

        // Assert
        Assert.That(writer.ToString(), Is.EqualTo("\tname?: string;"));
    }

    [Test]
    public async Task Generate_Property_Is_Not_Nullable()
    {
        // Arrange
        var property = new TypeScriptProperty("Name", new TypeScriptSystemType("string"), false);
        var writer = new TypeScriptStringWriter();

        // Act
        await PropertyGenerator.Generate(writer, property);

        // Assert
        Assert.That(writer.ToString(), Is.EqualTo("\tname: string;"));
    }

    [Test]
    public async Task Generate_Array_Property()
    {
        // Arrange
        var property = new TypeScriptArrayProperty("Names", new TypeScriptSystemType("string"), false);
        var writer = new TypeScriptStringWriter();

        // Act
        await PropertyGenerator.Generate(writer, property);

        // Assert
        Assert.That(writer.ToString(), Is.EqualTo("\tnames: string[];"));
    }

    [Test]
    public async Task Generate_Dictionary_Property()
    {
        // Arrange
        var property = new TypeScriptDictionaryProperty("Names", new TypeScriptSystemType("string"),
            new TypeScriptSystemType("number"), false);
        var writer = new TypeScriptStringWriter();

        // Act
        await PropertyGenerator.Generate(writer, property);

        // Assert
        Assert.That(writer.ToString(), Is.EqualTo("\tnames: { [key: string]: number };"));
    }
}