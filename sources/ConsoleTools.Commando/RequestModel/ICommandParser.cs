namespace DustInTheWind.ConsoleTools.Commando.RequestModel;

public interface ICommandParser
{
    CommandRequest Parse(string[] args);
}