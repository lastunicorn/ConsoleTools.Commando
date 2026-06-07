namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal enum RequestMatchType
{
    /// <summary>
    /// No command is matching the request.
    /// </summary>
    NoMatch = 0,

    /// <summary>
    /// The command has optional parameters that are not matched by the provided arguments OR
    /// there are arguments from the request that remain unused.
    /// </summary>
    Partial,

    /// <summary>
    /// The command has all the parameters matched by the provided arguments and no arguments from th request remain unused.
    /// </summary>
    Full,

    /// <summary>
    /// Multiple commands fully match the request or, if there is no full match, multiple commands partially match the request.
    /// </summary>
    Multiple,

    /// <summary>
    /// Only the help command matches the request.
    /// </summary>
    Help,

    /// <summary>
    /// The command name matches but not the parameters.
    /// </summary>
    OnlyName
}