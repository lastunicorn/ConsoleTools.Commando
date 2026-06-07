using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class CommandsAnalysis
{
    public List<CommandAnalysis> FullMatches { get; } = new();

    public List<CommandAnalysis> PartialMatches { get; } = new();

    public List<CommandAnalysis> NameMatches { get; } = new();

    public CommandsAnalysis(CommandRequest commandRequest, IEnumerable<CommandMetadata> commandMetadataCollection)
    {
        foreach (CommandMetadata commandMetadata in commandMetadataCollection)
            Analyze(commandRequest, commandMetadata);
    }

    private void Analyze(CommandRequest commandRequest, CommandMetadata commandMetadata)
    {
        CommandAnalysis commandAnalysis = new(commandRequest, commandMetadata);

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

        if (commandAnalysis.Command.Name == commandRequest.CommandName)
            NameMatches.Add(commandAnalysis);
    }
}