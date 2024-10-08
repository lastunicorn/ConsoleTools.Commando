﻿// ConsoleTools.Commando
// Copyright (C) 2022-2024 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Reflection;
using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando;

public class Application
{
    private readonly CommandRouter commandRouter;
    private readonly ICommandParser commandParser;

    public string Name { get; set; }

    public event EventHandler Starting;

    public event EventHandler Ended;

    public event EventHandler<UnhandledApplicationExceptionEventArgs> UnhandledApplicationException;

    public Application(ICommandParser commandParser, CommandRouter commandRouter)
    {
        this.commandParser = commandParser ?? throw new ArgumentNullException(nameof(commandParser));
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

            CommandRequest commandRequest = commandParser.Parse(args);
            await commandRouter.Execute(commandRequest);
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