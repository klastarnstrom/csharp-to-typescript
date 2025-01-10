using CSharpToTypeScript.Library.Generators;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Tests.Generators;

[TestFixture]
public class EnumGeneratorTests
{
    [Test]
    public async Task Generate_EmptyEnum_Returns_Empty_Enum()
    {
        // Arrange
        var enumModel = new TypeScriptEnum("EmptyEnum", new());
        
        // Act
        var result = await EnumGenerator.Generate(enumModel);
        
        // Assert
        Assert.That(result, Is.EqualTo("enum EmptyEnum {\n}"));
    }
    
    [Test]
    public async Task Generate_EnumWithValues_Returns_EnumWithValues()
    {
        // Arrange
        var enumModel = new TypeScriptEnum("EnumWithValues", ["Value1", "Value2"]);
        
        // Act
        var result = await EnumGenerator.Generate(enumModel);
        
        // Assert
        Assert.That(result, Is.EqualTo("enum EnumWithValues {\n\tValue1 = \"Value1\",\n\tValue2 = \"Value2\",\n}"));
    }
}