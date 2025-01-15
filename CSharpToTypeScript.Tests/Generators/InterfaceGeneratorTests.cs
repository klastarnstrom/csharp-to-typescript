using CSharpToTypeScript.Library.Generators;
using CSharpToTypeScript.Library.Models;

namespace CSharpToTypeScript.Tests.Generators;

[TestFixture]
public class InterfaceGeneratorTests
{
    [Test]
    public async Task Generate_Empty_Interface()
    {
        // Arrange
        const string interfaceName = "EmptyInterface";
        var interfaceModel = new TypeScriptInterface(interfaceName);

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result, Is.EqualTo($"interface {interfaceName} {{}}"));
    }

    [Test]
    public async Task Generate_Interface_With_BaseType()
    {
        // Arrange
        const string baseTypeName = "BaseType";
        var baseType = new TypeScriptInterface(baseTypeName);

        const string interfaceName = "InterfaceWithBaseType";
        var interfaceModel = new TypeScriptInterface(interfaceName)
        {
            BaseType = baseType
        };

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result, Is.EqualTo($"interface {interfaceName} extends {baseTypeName} {{}}"));
    }

    [Test]
    public async Task Generate_Interface_With_Implemented_Interfaces()
    {
        // Arrange

        const string implementedInterfaceName = "InterfaceWithImplementedInterfaces";
        var implementedInterface = new TypeScriptInterface(implementedInterfaceName);

        const string interfaceName = "InterfaceWithImplementedInterfaces";

        var interfaceModel = new TypeScriptInterface(interfaceName);

        interfaceModel.ImplementedInterfaces.Add(implementedInterface);

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result, Is.EqualTo($"interface {interfaceName} extends {implementedInterfaceName} {{}}"));
    }

    [Test]
    public async Task Generate_Interface_With_BaseType_And_Implemented_Interfaces()
    {
        // Arrange
        const string baseTypeName = "BaseType";
        var baseType = new TypeScriptInterface(baseTypeName);

        const string implementedInterfaceName = "ImplementedInterface";
        var implementedInterface = new TypeScriptInterface(implementedInterfaceName);

        const string interfaceName = "InterfaceWithBaseTypeAndImplementedInterfaces";
        var interfaceModel = new TypeScriptInterface(interfaceName)
        {
            BaseType = baseType
        };

        interfaceModel.ImplementedInterfaces.Add(implementedInterface);

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo($"interface {interfaceName} extends {baseTypeName}, {implementedInterfaceName} {{}}"));
    }

    [Test]
    public async Task Generate_Interface_With_GenericArguments()
    {
        // Arrange
        const string interfaceName = "InterfaceWithGenericArguments";
        var interfaceModel = new TypeScriptInterface(interfaceName);

        const string genericParameterName = "T";
        var genericParameter = new TypeScriptInterface(genericParameterName);

        interfaceModel.GenericArguments.Add(genericParameter);

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result, Is.EqualTo($"interface {interfaceName}<{genericParameterName}> {{}}"));
    }

    [Test]
    public async Task Generate_Interface_With_BaseType_And_GenericArguments()
    {
        // Arrange
        const string baseTypeName = "BaseType";
        var baseType = new TypeScriptInterface(baseTypeName);

        const string interfaceName = "InterfaceWithBaseTypeAndGenericArguments";
        var interfaceModel = new TypeScriptInterface(interfaceName)
        {
            BaseType = baseType
        };

        const string genericParameterName = "T";
        var genericParameter = new TypeScriptInterface(genericParameterName);

        baseType.GenericArguments.Add(genericParameter);

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo($"interface {interfaceName} extends {baseTypeName}<{genericParameterName}> {{}}"));
    }

    [Test]
    public async Task Generate_Interface_With_Implemented_Interfaces_And_GenericArguments()
    {
        // Arrange
        const string implementedInterfaceName = "ImplementedInterface";
        var implementedInterface = new TypeScriptInterface(implementedInterfaceName);

        const string interfaceName = "InterfaceWithImplementedInterfacesAndGenericArguments";
        var interfaceModel = new TypeScriptInterface(interfaceName);

        interfaceModel.ImplementedInterfaces.Add(implementedInterface);

        const string genericParameterName = "T";
        var genericParameter = new TypeScriptInterface(genericParameterName);

        implementedInterface.GenericArguments.Add(genericParameter);

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo($"interface {interfaceName} extends {implementedInterfaceName}<{genericParameterName}> {{}}"));
    }

    [Test]
    public async Task
        Generate_Interface_With_BaseType_And_Implemented_Interfaces_And_GenericArguments()
    {
        // Arrange
        const string baseTypeName = "BaseType";
        var baseType = new TypeScriptInterface(baseTypeName);

        const string implementedInterfaceName = "ImplementedInterface";
        var implementedInterface = new TypeScriptInterface(implementedInterfaceName);

        const string interfaceName = "InterfaceWithBaseTypeAndImplementedInterfacesAndGenericArguments";
        var interfaceModel = new TypeScriptInterface(interfaceName)
        {
            BaseType = baseType
        };

        interfaceModel.ImplementedInterfaces.Add(implementedInterface);

        const string genericParameterName = "T";
        var genericParameter = new TypeScriptInterface(genericParameterName);

        implementedInterface.GenericArguments.Add(genericParameter);

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo(
                $"interface {interfaceName} extends {baseTypeName}, {implementedInterfaceName}<{genericParameterName}> {{}}"));
    }

    [Test]
    public async Task Generate_Interface_With_Properties()
    {
        // Arrange
        const string interfaceName = "InterfaceWithProperties";
        var interfaceModel = new TypeScriptInterface(interfaceName);

        interfaceModel.Properties.Add(new("Property1", TypeScriptSystemType.Create(typeof(string)), true));
        interfaceModel.Properties.Add(new("Property2", TypeScriptSystemType.Create(typeof(int)), false));

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo($"interface {interfaceName} {{\n\tproperty1?: string;\n\tproperty2: number;\n}}"));
    }

    [Test]
    public async Task Generate_Interface_With_Base_Type_And_Properties()
    {
        // Arrange
        const string baseTypeName = "BaseType";
        var baseType = new TypeScriptInterface(baseTypeName);

        const string interfaceName = "InterfaceWithBaseTypeAndProperties";
        var interfaceModel = new TypeScriptInterface(interfaceName)
        {
            BaseType = baseType
        };

        interfaceModel.Properties.Add(new("Property1", TypeScriptSystemType.Create(typeof(string)), true));
        interfaceModel.Properties.Add(new("Property2", TypeScriptSystemType.Create(typeof(int)), false));

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo(
                $"interface {interfaceName} extends {baseTypeName} {{\n\tproperty1?: string;\n\tproperty2: number;\n}}"));
    }

    [Test]
    public async Task Generate_Interface_With_Implemented_Interfaces_And_Properties()
    {
        // Arrange
        const string implementedInterfaceName = "ImplementedInterface";
        var implementedInterface = new TypeScriptInterface(implementedInterfaceName);

        const string interfaceName = "InterfaceWithImplementedInterfacesAndProperties";
        var interfaceModel = new TypeScriptInterface(interfaceName);

        interfaceModel.ImplementedInterfaces.Add(implementedInterface);

        interfaceModel.Properties.Add(new("Property1", TypeScriptSystemType.Create(typeof(string)), true));
        interfaceModel.Properties.Add(new("Property2", TypeScriptSystemType.Create(typeof(int)), false));

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo(
                $"interface {interfaceName} extends {implementedInterfaceName} {{\n\tproperty1?: string;\n\tproperty2: number;\n}}"));
    }

    [Test]
    public async Task Generate_Interface_With_Base_Type_And_Implemented_Interfaces_And_Properties()
    {
        // Arrange
        const string baseTypeName = "BaseType";
        var baseType = new TypeScriptInterface(baseTypeName);

        const string implementedInterfaceName = "ImplementedInterface";
        var implementedInterface = new TypeScriptInterface(implementedInterfaceName);

        const string interfaceName = "InterfaceWithBaseTypeAndImplementedInterfacesAndProperties";
        var interfaceModel = new TypeScriptInterface(interfaceName)
        {
            BaseType = baseType
        };

        interfaceModel.ImplementedInterfaces.Add(implementedInterface);

        interfaceModel.Properties.Add(new("Property1", TypeScriptSystemType.Create(typeof(string)), true));
        interfaceModel.Properties.Add(new("Property2", TypeScriptSystemType.Create(typeof(int)), false));

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo(
                $"interface {interfaceName} extends {baseTypeName}, {implementedInterfaceName} {{\n\tproperty1?: string;\n\tproperty2: number;\n}}"));
    }

    [Test]
    public async Task Generate_Interface_With_Generic_Arguments_And_Properties()
    {
        // Arrange
        const string interfaceName = "InterfaceWithGenericArgumentsAndProperties";
        var interfaceModel = new TypeScriptInterface(interfaceName);

        const string genericParameterName = "T";
        var genericParameter = new TypeScriptInterface(genericParameterName);

        interfaceModel.GenericArguments.Add(genericParameter);

        interfaceModel.Properties.Add(new("Property1", TypeScriptSystemType.Create(typeof(string)), true));
        interfaceModel.Properties.Add(new("Property2", TypeScriptSystemType.Create(typeof(int)), false));

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo(
                $"interface {interfaceName}<{genericParameterName}> {{\n\tproperty1?: string;\n\tproperty2: number;\n}}"));
    }

    [Test]
    public async Task Generate_Interface_With_Base_Type_And_Generic_Arguments_And_Properties()
    {
        // Arrange
        const string baseTypeName = "BaseType";
        var baseType = new TypeScriptInterface(baseTypeName);

        const string interfaceName = "InterfaceWithBaseTypeAndGenericArgumentsAndProperties";
        var interfaceModel = new TypeScriptInterface(interfaceName)
        {
            BaseType = baseType
        };

        const string genericParameterName = "T";
        var genericParameter = new TypeScriptInterface(genericParameterName);

        baseType.GenericArguments.Add(genericParameter);

        interfaceModel.Properties.Add(new("Property1", TypeScriptSystemType.Create(typeof(string)), true));
        interfaceModel.Properties.Add(new("Property2", TypeScriptSystemType.Create(typeof(int)), false));

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo(
                $"interface {interfaceName} extends {baseTypeName}<{genericParameterName}> {{\n\tproperty1?: string;\n\tproperty2: number;\n}}"));
    }

    [Test]
    public async Task Generate_Interface_With_Implemented_Interfaces_And_Generic_Arguments_And_Properties()
    {
        // Arrange
        const string implementedInterfaceName = "ImplementedInterface";
        var implementedInterface = new TypeScriptInterface(implementedInterfaceName);

        const string interfaceName = "InterfaceWithImplementedInterfacesAndGenericArgumentsAndProperties";
        var interfaceModel = new TypeScriptInterface(interfaceName);

        interfaceModel.ImplementedInterfaces.Add(implementedInterface);

        const string genericParameterName = "T";
        var genericParameter = new TypeScriptInterface(genericParameterName);

        implementedInterface.GenericArguments.Add(genericParameter);

        interfaceModel.Properties.Add(new("Property1", TypeScriptSystemType.Create(typeof(string)), true));
        interfaceModel.Properties.Add(new("Property2", TypeScriptSystemType.Create(typeof(int)), false));

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo(
                $"interface {interfaceName} extends {implementedInterfaceName}<{genericParameterName}> {{\n\tproperty1?: string;\n\tproperty2: number;\n}}"));
    }

    [Test]
    public async Task
        Generate_Interface_With_Base_Type_And_Implemented_Interfaces_And_Generic_Arguments_And_Properties()
    {
        // Arrange
        const string baseTypeName = "BaseType";
        var baseType = new TypeScriptInterface(baseTypeName);

        const string implementedInterfaceName = "ImplementedInterface";
        var implementedInterface = new TypeScriptInterface(implementedInterfaceName);

        const string interfaceName = "InterfaceWithBaseTypeAndImplementedInterfacesAndGenericArgumentsAndProperties";
        var interfaceModel = new TypeScriptInterface(interfaceName)
        {
            BaseType = baseType
        };

        interfaceModel.ImplementedInterfaces.Add(implementedInterface);

        const string genericParameterName = "T";
        var genericParameter = new TypeScriptInterface(genericParameterName);

        implementedInterface.GenericArguments.Add(genericParameter);

        interfaceModel.Properties.Add(new("Property1", TypeScriptSystemType.Create(typeof(string)), true));
        interfaceModel.Properties.Add(new("Property2", TypeScriptSystemType.Create(typeof(int)), false));

        // Act
        var result = await InterfaceGenerator.Generate(interfaceModel);

        // Assert
        Assert.That(result,
            Is.EqualTo(
                $"interface {interfaceName} extends {baseTypeName}, {implementedInterfaceName}<{genericParameterName}> {{\n\tproperty1?: string;\n\tproperty2: number;\n}}"));
    }
}