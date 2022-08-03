using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustInTheWind.ConsoleTools.Commando
{
    public class Application
    {
        private readonly CommandRouter commandRouter;

        public Application(CommandRouter commandRouter)
        {
            this.commandRouter = commandRouter ?? throw new ArgumentNullException(nameof(commandRouter));
            commandRouter.CommandCreated += HandleCommandCreated;
        }

        private static void HandleCommandCreated(object? sender, CommandCreatedEventArgs e)
        {
            if (e.UnusedArguments.Count <= 0)
                return;

            IEnumerable<string> unusedArguments = e.UnusedArguments
                .Select(x => x.Name ?? x.Value);

            foreach (string unusedArgument in unusedArguments)
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, $"Unknown argument: {unusedArgument}");
        }

        public async Task Run(string[] args)
        {
            Arguments arguments = new(args);
            await commandRouter.Execute(arguments);
        }
    }
}