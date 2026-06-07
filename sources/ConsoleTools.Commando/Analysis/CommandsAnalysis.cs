using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class CommandsAnalysis
{
    public List<CommandMatch> FullMatches { get; } = new();

    public List<CommandMatch> PartialMatches { get; } = new();

    public List<CommandMatch> NameMatches { get; } = new();

    public CommandsAnalysis(XCommand xCommand, IEnumerable<CommandMetadata> commandMetadataCollection)
    {
        foreach (CommandMetadata commandMetadata in commandMetadataCollection)
            Analyze(xCommand, commandMetadata);
    }

    private void Analyze(XCommand xCommand, CommandMetadata commandMetadata)
    {
        CommandMatch commandMatch = new(xCommand, commandMetadata);

        switch (commandMatch.MatchType)
        {
            case CommandMatchType.NoMatch:
                break;

            case CommandMatchType.Partial:
                PartialMatches.Add(commandMatch);
                break;

            case CommandMatchType.Full:
                FullMatches.Add(commandMatch);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        if (commandMatch.Command.Name == xCommand.Name)
            NameMatches.Add(commandMatch);
    }
}