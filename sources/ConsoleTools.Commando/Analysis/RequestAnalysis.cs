using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class RequestAnalysis
{
    private CommandMatch matchedCommandMatch;

    public CommandMetadata MatchedCommand => matchedCommandMatch.Command;
    
    public UnusedArguments UnusedArguments => matchedCommandMatch.UnusedArguments;

    public RequestMatchType MatchType { get; private set; }

    public List<ParameterMatch> UnmatchedMandatoryParameters { get; } = new();

    public RequestAnalysis(XCommand xCommand, MetadataContext metadataContext)
    {
        if (xCommand == null) throw new ArgumentNullException(nameof(xCommand));
        if (metadataContext == null) throw new ArgumentNullException(nameof(metadataContext));

        matchedCommandMatch = null;
        MatchType = RequestMatchType.NoMatch;

        if (xCommand.IsEmpty)
        {
            CommandMetadata commandMetadata = metadataContext.Commands.GetHelpCommand();

            matchedCommandMatch = new CommandMatch(xCommand, commandMetadata);
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
                        matchedCommandMatch = commandsAnalysis.PartialMatches.Single();
                        MatchType = RequestMatchType.Partial;
                        break;

                    default:
                        MatchType = RequestMatchType.Multiple;
                        break;
                }
                break;

            case 1:
                matchedCommandMatch = commandsAnalysis.FullMatches.Single();
                MatchType = RequestMatchType.Full;
                break;

            default:
                MatchType = RequestMatchType.Multiple;
                break;
        }
    }

    public void SetParameters(object consoleCommand)
    {
        matchedCommandMatch?.SetParameters(consoleCommand);
    }
}