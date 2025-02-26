using CSharpToTypeScript.Library.Exports.Types;
using CSharpToTypeScript.Tests.TestModels;

namespace CSharpToTypeScript.Tests.Exports.Types;

[TestFixture]
public class TsEnumTests
{
    [Test]
    public void Export_Generates_Valid_Ts_Enum()
    {
        // Arrange
        var tsEnum = new TsEnum(typeof(TestEnum));

        // Act
        var result = tsEnum.Export();

        // Assert
        const string enumName = nameof(TestEnum);
        const string value1Name = nameof(TestEnum.Value1);
        const string value2Name = nameof(TestEnum.Value2);
        const string value3Name = nameof(TestEnum.Value3);
        
        const string expected = $"export enum {enumName} {{\n\t{value1Name} = \"{value1Name}\",\n\t{value2Name} = \"{value2Name}\",\n\t{value3Name} = \"{value3Name}\",\n}}\n";

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Export_Generates_Empty_Enum_When_No_Values_In_Enum()
    {
        // Arrange
        var tsEnum = new TsEnum(typeof(EmptyEnum));

        // Act
        var result = tsEnum.Export();

        // Assert
        const string expected = "export enum EmptyEnum {\n}\n";

        Assert.That(result, Is.EqualTo(expected));
    }
}