using CSharpToTypeScript.Library.Generators;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Tests.Generators;

[TestFixture]
public class TypeNameGeneratorTests
{
    [Test]
    public async Task Generate_TypeScriptType_Returns_TypeName()
    {
        // Arrange
        var writer = new TypeScriptStringWriter();

        const string typeName = "string";
        var type = new TypeScriptSystemType(typeName);
        
        // Act
        await TypeNameGenerator.Generate(writer, type);
        
        // Assert
        Assert.That(writer.ToString(), Is.EqualTo(typeName));
    }
    
    [Test]
    public async Task Generate_TypeScriptInterface_Returns_TypeName_With_GenericArguments()
    {
        // Arrange
        var writer = new TypeScriptStringWriter();

        const string typeName = "TypeWithGenericArguments";
        var type = new TypeScriptInterface(typeName);
        
        type.GenericArguments.Add(new TypeScriptSystemType("string"));
        
        // Act
        await TypeNameGenerator.Generate(writer, type);
        
        // Assert
        Assert.That(writer.ToString(), Is.EqualTo($"{typeName}<string>"));
    }
    
    [Test]
    public async Task Generate_TypeScriptInterface_Returns_TypeName_With_Multiple_GenericArguments()
    {
        // Arrange
        var writer = new TypeScriptStringWriter();

        const string typeName = "TypeWithMultipleGenericArguments";
        var type = new TypeScriptInterface(typeName);
        
        type.GenericArguments.Add(new TypeScriptSystemType("string"));
        type.GenericArguments.Add(new TypeScriptSystemType("number"));
        
        // Act
        await TypeNameGenerator.Generate(writer, type);
        
        // Assert
        Assert.That(writer.ToString(), Is.EqualTo($"{typeName}<string, number>"));
    }
}