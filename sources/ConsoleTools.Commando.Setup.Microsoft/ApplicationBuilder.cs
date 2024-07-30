// ConsoleTools.Commando
// Copyright (C) 2022-2023 Dust in the Wind
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
using DustInTheWind.ConsoleTools.Commando.Parsing;
using Microsoft.Extensions.DependencyInjection;
using ExecutionContext = DustInTheWind.ConsoleTools.Commando.MetadataModel.ExecutionContext;

namespace DustInTheWind.ConsoleTools.Commando.Setup.Microsoft;

public class ApplicationBuilder
{
    private readonly IServiceCollection serviceCollection;
    private readonly ExecutionContext executionContext;
    private bool isCommandParserConfigured;
    private EventHandler<UnhandledApplicationExceptionEventArgs> unhandledExceptionHandler;

    public ApplicationBuilder()
    {
        serviceCollection = new ServiceCollection();
        executionContext = new ExecutionContext();

        ConfigureDefaultServices();
        LoadDefaultCommands();
    }

    public static ApplicationBuilder Create()
    {
        return new ApplicationBuilder();
    }

    private void ConfigureDefaultServices()
    {
        serviceCollection.AddTransient<EnhancedConsole>();

        serviceCollection.AddTransient<CommandRouter>();
        serviceCollection.AddTransient<ICommandFactory, CommandFactory>();

        serviceCollection.AddSingleton(executionContext);

        serviceCollection.AddSingleton<Application>();
    }

    private void LoadDefaultCommands()
    {
        executionContext.LoadFromAssemblyContaining<ExecutionContext>();
    }

    public ApplicationBuilder RegisterCommandsFrom(Func<Assembly> assemblyProvider)
    {
        if (assemblyProvider == null) throw new ArgumentNullException(nameof(assemblyProvider));

        Assembly assembly = assemblyProvider();
        executionContext.LoadFrom(assembly);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFrom(Func<IEnumerable<Assembly>> assemblyProvider)
    {
        if (assemblyProvider == null) throw new ArgumentNullException(nameof(assemblyProvider));

        Assembly[] assemblies = assemblyProvider().ToArray();
        executionContext.LoadFrom(assemblies);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFrom(params Assembly[] assemblies)
    {
        executionContext.LoadFrom(assemblies);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFromAssemblyContaining(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        executionContext.LoadFrom(type.Assembly);

        return this;
    }

    public ApplicationBuilder UseCommandParser(Type commandParserType)
    {
        if (commandParserType == null) throw new ArgumentNullException(nameof(commandParserType));

        bool typeIsCommandParser = typeof(ICommandParser).IsAssignableFrom(commandParserType);

        if (!typeIsCommandParser)
        {
            string typeFullName = commandParserType.FullName;
            string commandParserTypeFullName = typeof(ICommandParser).FullName;
            string message = $"Type {typeFullName} does not represent a command parser. A command parser must implement the {commandParserTypeFullName} interface.";

            throw new ArgumentException(message, nameof(commandParserType));
        }

        serviceCollection.AddTransient(typeof(ICommandParser), commandParserType);

        isCommandParserConfigured = true;

        return this;
    }

    public ApplicationBuilder ConfigureServices(Action<IServiceCollection> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        action(serviceCollection);

        return this;
    }

    public ApplicationBuilder HandleExceptions(EventHandler<UnhandledApplicationExceptionEventArgs> eventHandler)
    {
        unhandledExceptionHandler = eventHandler;

        return this;
    }

    public Application Build()
    {
        IServiceProvider serviceProvider = FinalizeContainerSetup();
        Application application = serviceProvider.GetService<Application>();

        if (unhandledExceptionHandler != null)
            application.UnhandledApplicationException += unhandledExceptionHandler;

        return application;
    }

    private IServiceProvider FinalizeContainerSetup()
    {
        if (!isCommandParserConfigured)
            serviceCollection.AddTransient(typeof(ICommandParser), typeof(CommandParser));

        executionContext.Freeze();

        foreach (Type type in executionContext.Commands.GetCommandTypes())
            serviceCollection.AddTransient(type);

        foreach (Type type in executionContext.Views.GetViewTypes())
            serviceCollection.AddTransient(type);

        return serviceCollection.BuildServiceProvider();
    }
}