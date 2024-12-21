namespace CsharpToTypeScript.Library.TypeGenerators.Base;

public interface ITypeDeclarationGenerator 
{
    string Generate(Type type);
}