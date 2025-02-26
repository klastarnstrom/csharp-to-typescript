using System.Reflection;
using System.Reflection.Emit;
using CSharpToTypeScript.Library.Attributes;

namespace CSharpToTypeScript.Tests.Base;

public class BaseTest
{
    protected AssemblyBuilder AssemblyBuilder;
    private ModuleBuilder _moduleBuilder;
    
    private const string AssemblyName = "DynamicTestAssembly";
    private const string ModuleName = "DynamicTestModule";

    [SetUp]
    public void BaseSetUp()
    {
        var assemblyName = new AssemblyName(AssemblyName);
        AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        _moduleBuilder = AssemblyBuilder.DefineDynamicModule(ModuleName);
    }
    
    protected TypeBuilder CreateTestType(string name, bool isPublic = true)
    {
        var typeAttributes = isPublic 
            ? TypeAttributes.Public | TypeAttributes.Class 
            : TypeAttributes.NotPublic | TypeAttributes.Class;
            
        var typeBuilder = _moduleBuilder.DefineType(name, typeAttributes);
        var attributeConstructor = typeof(TsGenerateAttribute).GetConstructor([]);
        typeBuilder.SetCustomAttribute(new(attributeConstructor!, []));
        
        return typeBuilder;
    }
}