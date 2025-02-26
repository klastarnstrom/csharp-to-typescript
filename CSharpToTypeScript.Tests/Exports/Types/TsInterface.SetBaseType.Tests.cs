using CSharpToTypeScript.Library.Exports.Types;
using CSharpToTypeScript.Tests.TestModels;

namespace CSharpToTypeScript.Tests.Exports.Types;

[TestFixture]
public partial class TsInterfaceTests
{
    [Test]
    public void Throws_When_Trying_To_Add_Base_Type_ToInterface()
        => Assert.Throws<InvalidOperationException>(() =>
            new TsInterface(typeof(ISimpleInterface)).SetBaseType(typeof(IImplementedInterface)));
    
    [Test]
    public void Throws_When_Trying_To_Add_Interface_As_BaseType()
        => Assert.Throws<InvalidOperationException>(() =>
            new TsInterface(typeof(SimpleClass)).SetBaseType(typeof(ISimpleInterface)));
    
    [Test]
    public void Throws_When_Trying_To_Add_Enum_As_BaseType()
        => Assert.Throws<InvalidOperationException>(() =>
            new TsInterface(typeof(SimpleClass)).SetBaseType(typeof(TestEnum)));
}