using CSharpToTypeScript.Library.Exports.Types;
using CSharpToTypeScript.Tests.Base;

namespace CSharpToTypeScript.Tests.Exports.Types;

[TestFixture]
public partial class TsInterfaceTests : BaseExportTest
{
    // Test types
    private interface ISimpleInterface;

    private class SimpleClass;

    private class DerivedClass : SimpleClass;

    private class DerivedClassWithInterface : SimpleClass, ISimpleInterface;

    private interface IImplementedInterface;

    private interface IWithProperties
    {
        string StringProperty { get; set; }
        int NumberProperty { get; set; }
        bool? NullableProperty { get; set; }
    }

    [Test]
    public void Empty_Interface_Should_Export_Correctly()
    {
        // Arrange
        var tsInterface = new TsInterface(typeof(ISimpleInterface));

        // Act
        var result = tsInterface.Export();

        // Assert

        const string interfaceName = nameof(ISimpleInterface);

        Writer.WriteLine($$"""
                           export interface {{interfaceName}} {
                           }
                           """);

        var expected = Writer.ToString();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void InterfaceWithProperties_ShouldExportCorrectly()
    {
        // Arrange
        var tsInterface = new TsInterface(typeof(IWithProperties));

        // Get property info objects using reflection
        var stringProperty = typeof(IWithProperties).GetProperty("StringProperty");
        var numberProperty = typeof(IWithProperties).GetProperty("NumberProperty");
        var nullableProperty = typeof(IWithProperties).GetProperty("NullableProperty");

        Assume.That(stringProperty, Is.Not.Null);
        Assume.That(numberProperty, Is.Not.Null);
        Assume.That(nullableProperty, Is.Not.Null);

        // Add properties to the interface
        tsInterface.AddProperty(new(stringProperty));
        tsInterface.AddProperty(new(numberProperty));
        tsInterface.AddProperty(new(nullableProperty));

        // Act
        var result = tsInterface.Export();

        // Assert
        const string interfaceName = nameof(IWithProperties);
        const char tab = '\t';

        Writer.WriteLine($"export interface {interfaceName} {{");
        Writer.WriteLine($"{tab}StringProperty: string;");
        Writer.WriteLine($"{tab}NumberProperty: number;");
        Writer.WriteLine($"{tab}NullableProperty?: boolean;");
        Writer.WriteLine("}");
        var expected = Writer.ToString();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void InterfaceWithImplementedInterface_ShouldExportCorrectly()
    {
        // Arrange
        var tsInterface = new TsInterface(typeof(ISimpleInterface));
        tsInterface.AddImplementedInterface(typeof(IImplementedInterface));

        // Act
        var result = tsInterface.Export();

        // Assert
        const string interfaceName = nameof(ISimpleInterface);
        const string implementedName = nameof(IImplementedInterface);

        Writer.WriteLine($"export interface {interfaceName} extends {implementedName} {{");
        Writer.WriteLine("}");
        var expected = Writer.ToString();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Class_Should_Export_Correctly()
    {
        var tsInterface = new TsInterface(typeof(SimpleClass));

        // Act
        var result = tsInterface.Export();

        // Assert
        const string className = nameof(SimpleClass);

        Writer.WriteLine($$"""
                           export interface {{className}} {
                           }
                           """);
        
        var expected = Writer.ToString();

        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void Class_With_Base_Class_Should_Export_Correctly()
    {
        // Arrange
        var tsInterface = new TsInterface(typeof(DerivedClass));
        tsInterface.SetBaseType(typeof(SimpleClass));

        // Act
        var result = tsInterface.Export();

        // Assert
        const string className = nameof(DerivedClass);
        const string baseName = nameof(SimpleClass);

        Writer.WriteLine($"export interface {className} extends {baseName} {{");
        Writer.WriteLine("}");
        var expected = Writer.ToString();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Class_With_Base_Class_And_Implemented_Interface_Should_Export_Correctly()
    {
        // Arrange
        var tsInterface = new TsInterface(typeof(DerivedClassWithInterface));
        tsInterface.SetBaseType(typeof(SimpleClass));
        tsInterface.AddImplementedInterface(typeof(ISimpleInterface));

        // Act
        var result = tsInterface.Export();

        // Assert
        const string className = nameof(DerivedClassWithInterface);
        const string baseName = nameof(SimpleClass);
        const string implementedName = nameof(ISimpleInterface);

        Writer.WriteLine($"export interface {className} extends {baseName}, {implementedName} {{");
        Writer.WriteLine("}");
        var expected = Writer.ToString();

        Assert.That(result, Is.EqualTo(expected));
    }
}