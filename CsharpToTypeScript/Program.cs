using CSharpToTypeScript.ExampleData;
using CSharpToTypeScript.Library;

new TypeScriptGenerator().Generate(new()
{
    Assemblies = [typeof(TestClass).Assembly]
});