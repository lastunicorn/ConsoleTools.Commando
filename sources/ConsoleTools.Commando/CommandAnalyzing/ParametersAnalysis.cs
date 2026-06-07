namespace DustInTheWind.ConsoleTools.Commando.CommandAnalyzing;

internal class ParametersAnalysis
{
    public List<ParameterMatch> UnmatchedMandatory { get; } = new();

    public bool HasUnmatchedMandatory => UnmatchedMandatory.Count > 0;

    public bool HasUnmatchedOptional { get; }

    public ParametersAnalysis(IEnumerable<ParameterMatch> parameterMatches)
    {
        List<ParameterMatch> notMatched = parameterMatches
            .Where(x => !x.IsMatch)
            .ToList();

        foreach (ParameterMatch parameterMatch in notMatched)
        {
            if (parameterMatch.IsParameterMandatory)
                UnmatchedMandatory.Add(parameterMatch);
            else
                HasUnmatchedOptional = true;
        }
    }
}