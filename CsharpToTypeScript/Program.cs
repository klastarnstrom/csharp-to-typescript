using CSharpToTypeScript.Library;

var configuration = new TypeScriptConfiguration();

await new TypeScriptGenerator(configuration).Generate();