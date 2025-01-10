using System.Reflection;
using CSharpToTypeScript.Library;
using CSharpToTypeScript.Library.Attributes;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Tests;

[TestFixture]
public class EnumTests 
{
    [Test]
    public void Resolve_EnumType_Returns_TypeScriptEnum_With_Correct_Name()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var collector = new TypeCollector([assembly]);

        // Act
        var result = collector.Resolve().Result;

        // Assert
        var tsEnum = (TypeScriptEnum)result.First().Value;
        Assert.That(tsEnum.Name, Is.EqualTo(nameof(TestEnum)));
    }
    
    [Test]
    public void Resolve_EnumType_Returns_TypeScript_Enum_With_Correct_Enum_Values()
    {
        // Arrange
        var assembly = typeof(TestEnum).Assembly;
        var collector = new TypeCollector([assembly]);

        // Act
        var result = collector.Resolve().Result;

        // Assert
        var tsEnum = (TypeScriptEnum)result.First().Value;
        var expectedEnumValues = Enum.GetNames(typeof(TestEnum));
        
        Assert.That(tsEnum.EnumValues, Is.EquivalentTo(expectedEnumValues));
    }
    
    [TsGenerate]
    public enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }
}