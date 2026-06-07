namespace DustInTheWind.ConsoleTools.Commando;

public class TypeIsNotCommandException : Exception
{
    public TypeIsNotCommandException(Type type)
        : base(BuildMessage(type))
    {
    }

    private static string BuildMessage(Type type)
    {
        string typeFullName = type.FullName;
        string commandType1FullName = typeof(IConsoleCommand).FullName;
        string commandType2FullName = typeof(IConsoleCommand<>).FullName;

        return $"Type {typeFullName} does not represent a command. A command must implement the {commandType1FullName} or {commandType2FullName} interface.";
    }
}