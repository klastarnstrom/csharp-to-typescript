using CSharpToTypeScript.Library;
using CSharpToTypeScript.Tests.Base;

namespace CSharpToTypeScript.Tests;

[TestFixture]
public class TypeCollectorTests : BaseTest
{
    [Test]
    public void Returns_EmptyList_When_NoAssembliesProvided()
    {
        var result = TypeCollector.CollectTypes([]);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Returns_TypesList_When_Assemblies_Contain_Types_With_Attribute()
    {
        // Arrange
        var publicTypeBuilder = CreateTestType("PublicTestClass");
        var publicType = publicTypeBuilder.CreateType();

        // Act
        var result = TypeCollector.CollectTypes([AssemblyBuilder]);

        // Assert
        Assert.That(result, Contains.Item(publicType));
    }

    [Test]
    public void Returns_OnlyPublicTypes_When_AssemblyContainsMixedAccessibilityTypes()
    {
        // Arrange
        var publicTypeBuilder = CreateTestType("PublicTestClass");
        var internalTypeBuilder = CreateTestType("InternalTestClass", false);
        
        var publicType = publicTypeBuilder.CreateType();
        var internalType = internalTypeBuilder.CreateType();

        // Act
        var result = TypeCollector.CollectTypes([AssemblyBuilder]);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.All(type => !type.IsNotPublic), Is.True);
            Assert.That(result, Contains.Item(publicType));
        });
        
        Assert.That(result, Does.Not.Contain(internalType));
    }
    

}