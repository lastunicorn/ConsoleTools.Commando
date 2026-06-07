using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class CommandsAnalysis
{
    public List<CommandAnalysis> FullMatches { get; } = new();

    public List<CommandAnalysis> PartialMatches { get; } = new();

    public List<CommandAnalysis> NameMatches { get; } = new();

    public CommandsAnalysis(XCommand xCommand, IEnumerable<CommandMetadata> commandMetadataCollection)
    {
        foreach (CommandMetadata commandMetadata in commandMetadataCollection)
            Analyze(xCommand, commandMetadata);
    }

    private void Analyze(XCommand xCommand, CommandMetadata commandMetadata)
    {
        CommandAnalysis commandAnalysis = new(xCommand, commandMetadata);

        switch (commandAnalysis.MatchType)
        {
            case CommandMatchType.NoMatch:
                break;

            case CommandMatchType.Partial:
                PartialMatches.Add(commandAnalysis);
                break;

            case CommandMatchType.Full:
                FullMatches.Add(commandAnalysis);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        if (commandAnalysis.Command.Name == xCommand.Name)
            NameMatches.Add(commandAnalysis);
    }
}