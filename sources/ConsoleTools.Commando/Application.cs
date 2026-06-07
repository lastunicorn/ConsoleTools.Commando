using System.Reflection;
using DustInTheWind.ConsoleTools.Commando.Routing;
using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando;

public class Application
{
    private readonly CommandRouter commandRouter;
    private readonly ICliSyntax cliSyntax;

    public string Name { get; set; }

    public event EventHandler Starting;

    public event EventHandler Ended;

    public event EventHandler<UnhandledApplicationExceptionEventArgs> UnhandledApplicationException;

    public Application(ICliSyntax cliSyntax, CommandRouter commandRouter)
    {
        this.cliSyntax = cliSyntax ?? throw new ArgumentNullException(nameof(cliSyntax));
        this.commandRouter = commandRouter ?? throw new ArgumentNullException(nameof(commandRouter));

        commandRouter.CommandCreated += HandleCommandCreated;

        Assembly assembly = Assembly.GetEntryAssembly();
        AssemblyName assemblyName = assembly?.GetName();
        Name = assemblyName?.Name ?? string.Empty;
    }

    private static void HandleCommandCreated(object sender, CommandCreatedEventArgs e)
    {
        if (e.UnusedOptions.Count > 0)
        {
            IEnumerable<string> unusedArguments = e.UnusedOptions
                .Select(x => x.Name);

            foreach (string unusedArgument in unusedArguments)
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, $"Unknown argument: {unusedArgument}");
        }

        if (e.UnusedOperands.Count > 0)
        {
            foreach (string unusedArgument in e.UnusedOperands)
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, $"Unknown argument: {unusedArgument}");
        }
    }

    public async Task RunAsync(string[] args)
    {
        try
        {
            OnStarting();

            XCommand xCommand = cliSyntax.Parse(args);
            await commandRouter.Execute(xCommand);
        }
        catch (Exception ex)
        {
            UnhandledApplicationExceptionEventArgs eventArgs = new(ex);
            OnUnhandledException(eventArgs);

            if (!eventArgs.IsHandled)
            {
#if DEBUG
                CustomConsole.WriteLineError(ex);
#else
                CustomConsole.WriteLineError(ex.Message);
#endif
            }
        }
        finally
        {
            OnEnded();
        }
    }

    protected virtual void OnStarting()
    {
        Starting?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnEnded()
    {
        Ended?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnUnhandledException(UnhandledApplicationExceptionEventArgs e)
    {
        UnhandledApplicationException?.Invoke(this, e);
    }
}