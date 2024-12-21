using CsharpToTypeScript.ExampleData;
using CsharpToTypeScript.Library;

var configuration = new TypeScriptConfiguration
{
   IncludedNamespaces = ["CsharpToTypeScript.ExampleData"]
};

new TypeScriptGenerator(configuration).Generate();