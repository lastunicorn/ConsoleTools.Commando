namespace DustInTheWind.ConsoleTools.Commando;

/// <summary>
/// Represents a console command that handles a specific request from the user.
/// The correct command to be executed is identified based on the command line arguments provided
/// by the user in the console.
/// </summary>
public interface IConsoleCommand
{
    /// <summary>
    /// When implemented by an inheritor, it executes asynchronously the actions needed for handling
    /// the user's request.
    /// </summary>
    Task Execute();
}