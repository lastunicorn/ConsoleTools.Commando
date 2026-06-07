namespace DustInTheWind.ConsoleTools.Commando;

/// <summary>
/// Exception thrown when multiple commands match the parameters from the command line.
/// </summary>
public class MultipleCommandsMatchException : Exception
{
    private const string DefaultMessage = "Multiple commands match the provided parameters.";

    public MultipleCommandsMatchException()
        : base(DefaultMessage)
    {
    }
}