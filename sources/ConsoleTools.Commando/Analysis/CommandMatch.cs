using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class CommandMatch
{
    private readonly List<ParameterMatch> parameterMatches = new();

    public CommandMatchType MatchType { get; private set; }

    public CommandMetadata Command { get; }

    public List<ParameterMatch> UnmatchedMandatoryParameters { get; } = new();

    public UnusedArguments UnusedArguments { get; }

    public CommandMatch(XCommand xCommand, CommandMetadata commandMetadata)
    {
        if (xCommand == null) throw new ArgumentNullException(nameof(xCommand));
        Command = commandMetadata ?? throw new ArgumentNullException(nameof(commandMetadata));

        //xCommand.Reset();
        UnusedArguments = new UnusedArguments(xCommand.Arguments);

        AnalyzeParameters();
    }

    private void AnalyzeParameters()
    {
        IEnumerable<ParameterMatch> parameterMatchEnumeration = Command.EnumerateParameters()
            .Select(x => new ParameterMatch(x, UnusedArguments));

        bool hasUnmatchedMandatory = false;
        bool hasUnmatchedOptional = false;

        foreach (ParameterMatch parameterMatch in parameterMatchEnumeration)
        {
            parameterMatches.Add(parameterMatch);

            if (!parameterMatch.IsMatch)
            {
                if (parameterMatch.IsMandatory)
                {
                    UnmatchedMandatoryParameters.Add(parameterMatch);
                    hasUnmatchedMandatory = true;
                }
                else
                {
                    hasUnmatchedOptional = true;
                }
            }
        }

        if (hasUnmatchedMandatory)
            MatchType = CommandMatchType.NoMatch;
        else if (hasUnmatchedOptional)
            MatchType = CommandMatchType.Partial;
        else
            MatchType = CommandMatchType.Full;
    }

    public void SetParameters(object consoleCommand)
    {
        IEnumerable<ParameterMatch> validParameters = parameterMatches.Where(x => x.IsMatch);

        foreach (ParameterMatch parameterMatch in validParameters)
            parameterMatch.SetParameter(consoleCommand);
    }
}