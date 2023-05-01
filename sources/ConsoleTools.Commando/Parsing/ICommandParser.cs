using DustInTheWind.ConsoleTools.Commando.GenericCommandModel;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

public interface ICommandParser
{
    GenericCommand Parse(string[] args);
}