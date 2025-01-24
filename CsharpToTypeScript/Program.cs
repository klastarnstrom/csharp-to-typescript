using CSharpToTypeScript.ExampleData;
using CSharpToTypeScript.LibraryNew;

new TypeScriptGenerator().Generate(new()
{
    Assemblies = [typeof(TestClass).Assembly]
});