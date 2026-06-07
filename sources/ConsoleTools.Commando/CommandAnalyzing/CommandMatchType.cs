namespace DustInTheWind.ConsoleTools.Commando.CommandAnalyzing;

internal enum CommandMatchType
{
    /// <summary>
    /// The command does not match the request.
    /// </summary>
    NoMatch = 0,

    /// <summary>
    /// The command has optional parameters that are not matched by the provided arguments or
    /// there are arguments from the request that remain unused.
    /// </summary>
    Partial,

    /// <summary>
    /// The command has all the parameters matched by the provided arguments and
    /// no arguments from th request remain unused.
    /// </summary>
    Full
}