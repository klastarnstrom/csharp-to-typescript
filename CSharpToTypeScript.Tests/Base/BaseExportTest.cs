using CSharpToTypeScript.Library.Constants;

namespace CSharpToTypeScript.Tests.Base;

public class BaseExportTest 
{
    protected StringWriter Writer { get; private set; }

    [SetUp]
    public void BaseSetUp()
    {
        Writer = new() { NewLine = SpecialCharacters.UnixNewLine };
    }
    
    [TearDown]
    public void BaseTearDown()
    {
        Writer.Dispose();
    }
}