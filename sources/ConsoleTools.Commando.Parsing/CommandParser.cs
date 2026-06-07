using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

public class CommandParser : ICommandParser
{
    public CommandRequest Parse(string[] args)
    {
        if (args == null)
            return new CommandRequest();

        TextCommandAnalysis textCommandAnalysis = new(args);
        return textCommandAnalysis.Analyze();
    }
}