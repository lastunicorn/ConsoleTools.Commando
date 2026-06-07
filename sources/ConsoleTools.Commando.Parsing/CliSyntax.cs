using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

public class CliSyntax : ICliSyntax
{
    public XCommand Parse(string[] args)
    {
        if (args == null)
            return new XCommand();

        TextCommandAnalysis textCommandAnalysis = new(args);
        return textCommandAnalysis.Analyze();
    }
}