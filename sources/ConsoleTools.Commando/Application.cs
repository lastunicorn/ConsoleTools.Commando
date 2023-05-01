using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DustInTheWind.ConsoleTools.Commando.GenericCommandModel;
using DustInTheWind.ConsoleTools.Commando.Parsing;

namespace DustInTheWind.ConsoleTools.Commando;

public class Application
{
    private readonly CommandRouter commandRouter;
    private readonly ICommandParser commandParser;

    public string Name { get; set; }

    public Application(CommandRouter commandRouter, ICommandParser commandParser)
    {
        this.commandRouter = commandRouter ?? throw new ArgumentNullException(nameof(commandRouter));
        this.commandParser = commandParser ?? throw new ArgumentNullException(nameof(commandParser));
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

    public async Task Run(string[] args)
    {
        try
        {
            GenericCommand genericCommand = commandParser.Parse(args);
            await commandRouter.Execute(genericCommand);
        }
        catch (Exception ex)
        {
            CustomConsole.WriteError(ex.Message);
        }
    }
}