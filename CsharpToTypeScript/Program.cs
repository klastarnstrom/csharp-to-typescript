using CSharpToTypeScript.ExampleData;
using CSharpToTypeScript.Library;

var configuration = new TypeScriptConfiguration();

await new TypeScriptGenerator(configuration).Generate(typeof(TestClass).Assembly);