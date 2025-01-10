using CSharpToTypeScript.Library.Generators;
using CSharpToTypeScript.Library.Models;
using CSharpToTypeScript.Library.Models.Properties;

namespace CSharpToTypeScript.Tests.Generators;

[TestFixture]
public class BodyGeneratorTests 
{
    [Test]
    public async Task Generate_EmptyProperties_Returns_Empty_Body()
    {
        // Arrange
        var properties = new List<TypeScriptProperty>();
        var builder = new TypeScriptStringWriter();
        
        // Act
        await BodyGenerator.Generate(builder, properties);
        
        // Assert
        Assert.That(builder.ToString(), Is.EqualTo(" {}"));
    }
    
    [Test]
    public async Task Generate_Properties_Returns_Body_With_Properties()
    {
        // Arrange
        var properties = new List<TypeScriptProperty>
        {
            new("Name", new TypeScriptSystemType("string"), false),
            new("Age", new TypeScriptSystemType("number"), true)
        };
        
        var writer = new TypeScriptStringWriter();
        
        // Act
        await BodyGenerator.Generate(writer, properties);
        
        // Assert
        Assert.That(writer.ToString(), Is.EqualTo(" {\n\tname: string;\n\tage?: number;\n}"));
    }
}