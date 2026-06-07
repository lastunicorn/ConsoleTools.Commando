namespace DustInTheWind.ConsoleTools.Commando.Syntax;

public interface ICliSyntax
{
    XCommand Parse(string[] args);
}