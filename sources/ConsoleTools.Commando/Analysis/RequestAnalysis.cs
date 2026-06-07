using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class RequestAnalysis
{
    private CommandAnalysis matchedCommandAnalysis;

    public CommandMetadata MatchedCommand => matchedCommandAnalysis.Command;

    public RequestMatchType MatchType { get; private set; }

    public List<ParameterMatch> UnmatchedMandatoryParameters { get; } = new();

    public RequestAnalysis(XCommand xCommand, MetadataContext metadataContext)
    {
        if (xCommand == null) throw new ArgumentNullException(nameof(xCommand));
        if (metadataContext == null) throw new ArgumentNullException(nameof(metadataContext));

        matchedCommandAnalysis = null;
        MatchType = RequestMatchType.NoMatch;

        if (xCommand.IsEmpty)
        {
            CommandMetadata commandMetadata = metadataContext.Commands.GetHelpCommand();

            matchedCommandAnalysis = new CommandAnalysis(xCommand, commandMetadata);
            MatchType = RequestMatchType.Help;
        }
        else
        {
            IEnumerable<CommandMetadata> commandMetadataCollection = string.IsNullOrEmpty(xCommand.Name)
                ? metadataContext.Commands.GetAllAnonymous()
                : metadataContext.Commands.GetAllByName(xCommand.Name).ToList();

            Analyze(xCommand, commandMetadataCollection);
        }
    }

    private void Analyze(XCommand xCommand, IEnumerable<CommandMetadata> commandMetadataCollection)
    {
        CommandsAnalysis commandsAnalysis = new(xCommand, commandMetadataCollection);

        switch (commandsAnalysis.FullMatches.Count)
        {
            case 0:
                switch (commandsAnalysis.PartialMatches.Count)
                {
                    case 0:
                        if (commandsAnalysis.NameMatches.Count > 0)
                        {
                            MatchType = RequestMatchType.OnlyName;
                            UnmatchedMandatoryParameters.AddRange(commandsAnalysis.NameMatches.First().UnmatchedMandatoryParameters);
                        }
                        else
                        {
                            MatchType = RequestMatchType.NoMatch;
                        }
                        break;

                    case 1:
                        matchedCommandAnalysis = commandsAnalysis.PartialMatches.Single();
                        MatchType = RequestMatchType.Partial;
                        break;

                    default:
                        MatchType = RequestMatchType.Multiple;
                        break;
                }
                break;

            case 1:
                matchedCommandAnalysis = commandsAnalysis.FullMatches.Single();
                MatchType = RequestMatchType.Full;
                break;

            default:
                MatchType = RequestMatchType.Multiple;
                break;
        }
    }

    public void SetParameters(object consoleCommand)
    {
        matchedCommandAnalysis?.SetParameters(consoleCommand);
    }
}