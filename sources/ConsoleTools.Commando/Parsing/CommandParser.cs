using DustInTheWind.ConsoleTools.Commando.GenericCommandModel;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

public class CommandParser : ICommandParser
{
    public GenericCommand Parse(string[] args)
    {
        CommandAnalysis commandAnalysis = new(args);
        return commandAnalysis.Analyze();
    }
}