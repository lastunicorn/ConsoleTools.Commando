namespace DustInTheWind.ConsoleTools.Commando;

public class UnhandledApplicationExceptionEventArgs : EventArgs
{
    public Exception Exception { get; }

    public bool IsHandled { get; set; }

    public UnhandledApplicationExceptionEventArgs(Exception exception)
    {
        Exception = exception;
    }
}