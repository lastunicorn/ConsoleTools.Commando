using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DustInTheWind.ConsoleTools.Commando
{
    public class Application
    {
        private readonly CommandRouter commandRouter;

        public string Name { get; set; }

        public Application(CommandRouter commandRouter)
        {
            this.commandRouter = commandRouter ?? throw new ArgumentNullException(nameof(commandRouter));
            commandRouter.CommandCreated += HandleCommandCreated;

            Assembly assembly = Assembly.GetEntryAssembly();
            AssemblyName assemblyName = assembly?.GetName();
            Name = assemblyName?.Name ?? string.Empty;
        }

        private static void HandleCommandCreated(object sender, CommandCreatedEventArgs e)
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
            try
            {
                Arguments arguments = new(args);
                await commandRouter.Execute(arguments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}