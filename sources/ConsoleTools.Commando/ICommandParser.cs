using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando;

public interface ICommandParser
{
    CommandRequest Parse(string[] args);
}