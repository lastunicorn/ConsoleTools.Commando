using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class RequestAnalysis
{
    private CommandAnalysis matchedCommandAnalysis;

    public CommandMetadata MatchedCommand => matchedCommandAnalysis.Command;

    public RequestMatchType MatchType { get; private set; }

    public List<ParameterMatch> UnmatchedMandatoryParameters { get; } = new();

    public RequestAnalysis(CommandRequest commandRequest, MetadataContext metadataContext)
    {
        if (commandRequest == null) throw new ArgumentNullException(nameof(commandRequest));
        if (metadataContext == null) throw new ArgumentNullException(nameof(metadataContext));

        matchedCommandAnalysis = null;
        MatchType = RequestMatchType.NoMatch;

        if (commandRequest.IsEmpty)
        {
            CommandMetadata commandMetadata = metadataContext.Commands.GetHelpCommand();

            matchedCommandAnalysis = new CommandAnalysis(commandRequest, commandMetadata);
            MatchType = RequestMatchType.Help;
        }
        else
        {
            IEnumerable<CommandMetadata> commandMetadataCollection = string.IsNullOrEmpty(commandRequest.CommandName)
                ? metadataContext.Commands.GetAllAnonymous()
                : metadataContext.Commands.GetAllByName(commandRequest.CommandName).ToList();

            Analyze(commandRequest, commandMetadataCollection);
        }
    }

    private void Analyze(CommandRequest commandRequest, IEnumerable<CommandMetadata> commandMetadataCollection)
    {
        CommandsAnalysis commandsAnalysis = new(commandRequest, commandMetadataCollection);

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