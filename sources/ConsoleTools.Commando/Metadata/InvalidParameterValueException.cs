namespace DustInTheWind.ConsoleTools.Commando.Metadata;

internal class InvalidParameterValueException : Exception
{
    public InvalidParameterValueException(string parameterName, string value)
        : base(BuildMessage(parameterName, value))
    {
    }

    public InvalidParameterValueException(string parameterName, string value, Exception innerException)
        : base(BuildMessage(parameterName, value), innerException)
    {
    }

    private static string BuildMessage(string parameterName, string value)
    {
        return $"'{value}' is not a valid value for '{parameterName}'.";
    }
}