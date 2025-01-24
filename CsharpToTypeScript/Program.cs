using CSharpToTypeScript.ExampleData;
using CSharpToTypeScript.LibraryNew;

new TypeScriptGenerator().Generate([typeof(TestClass).Assembly]);

