namespace DustInTheWind.ConsoleTools.Commando;

internal class ParameterMissingException : Exception
{
    public ParameterMissingException(ICollection<string> parameterNames)
        : base(BuildMessage(parameterNames))
    {
    }

    private static string BuildMessage(ICollection<string> parameterNames)
    {
        switch (parameterNames.Count)
        {
            case 0:
                return Resources.ErrorMessage_ParameterMissing_0;

            case 1:
            {
                string parameterName = parameterNames.First();
                return string.Format(Resources.ErrorMessage_ParameterMissing_1, parameterName);
            }

            case > 1:
            {
                string parameterNamesAsString = string.Join(", ", parameterNames);
                return string.Format(Resources.ErrorMessage_ParameterMissing_N, parameterNamesAsString);
            }

            default:
                return Resources.ErrorMessage_ParameterMissing_0;
        }
    }
}