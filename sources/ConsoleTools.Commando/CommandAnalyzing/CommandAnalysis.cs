using DustInTheWind.ConsoleTools.Commando.MetadataModel;
using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando.CommandAnalyzing;

internal class CommandAnalysis
{
    private readonly List<ParameterMatch> parameterMatches;

    public CommandMatchType MatchType { get; }

    public CommandMetadata Command { get; set; }

    public List<ParameterMatch> UnmatchedMandatoryParameters { get; } = new();

    public CommandAnalysis(CommandRequest commandRequest, CommandMetadata commandMetadata)
    {
        if (commandRequest == null) throw new ArgumentNullException(nameof(commandRequest));
        Command = commandMetadata ?? throw new ArgumentNullException(nameof(commandMetadata));

        commandRequest.Reset();

        parameterMatches = commandMetadata.Parameters
            .Select(x => new ParameterMatch(x, commandRequest))
            .ToList();

        ParametersAnalysis parametersAnalysis = new(parameterMatches);

        if (parametersAnalysis.HasUnmatchedMandatory)
        {
            UnmatchedMandatoryParameters.AddRange(parametersAnalysis.UnmatchedMandatory);
            MatchType = CommandMatchType.NoMatch;
        }
        else if (parametersAnalysis.HasUnmatchedOptional)
        {
            MatchType = CommandMatchType.Partial;
        }
        else
        {
            MatchType = CommandMatchType.Full;
        }
    }

    public void SetParameters(object consoleCommand)
    {
        IEnumerable<ParameterMatch> validParameters = parameterMatches.Where(x => x.IsMatch);

        foreach (ParameterMatch parameterMatch in validParameters)
            parameterMatch.SetParameter(consoleCommand);
    }
}