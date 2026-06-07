namespace DustInTheWind.ConsoleTools.Commando.Commands.Empty;

[NamedCommand("empty", Enabled = false)]
internal class EmptyConsoleCommand : IConsoleCommand
{
    public Task Execute()
    {
        return Task.CompletedTask;
    }
}