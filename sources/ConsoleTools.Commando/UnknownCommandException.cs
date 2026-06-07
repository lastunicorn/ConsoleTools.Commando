namespace DustInTheWind.ConsoleTools.Commando;

public class UnknownCommandException : Exception
{
    public UnknownCommandException()
        : base(Resources.ErrorMessage_UnknownCommand)
    {
    }
}