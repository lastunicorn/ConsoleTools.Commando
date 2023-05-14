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
using DustInTheWind.ConsoleTools.Commando.CommandMetadataModel;
using DustInTheWind.ConsoleTools.Commando.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.ConsoleTools.Commando.Builder.Microsoft;

public class ApplicationBuilder
{
    private readonly IServiceCollection serviceCollection;
    private readonly CommandMetadataCollection commandMetadataCollection;
    private bool isCommandParserConfigured;

    public ApplicationBuilder()
    {
        serviceCollection = new ServiceCollection();
        commandMetadataCollection = new CommandMetadataCollection();

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

        serviceCollection.AddSingleton(commandMetadataCollection);

        serviceCollection.AddSingleton<Application>();
    }

    private void LoadDefaultCommands()
    {
        Assembly defaultCommandsAssembly = typeof(CommandMetadataCollection).Assembly;
        commandMetadataCollection.LoadFrom(defaultCommandsAssembly);
    }

    public ApplicationBuilder RegisterCommandsFrom(Func<Assembly> assemblyProvider)
    {
        if (assemblyProvider == null) throw new ArgumentNullException(nameof(assemblyProvider));

        Assembly assembly = assemblyProvider();
        commandMetadataCollection.LoadFrom(assembly);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFrom(Func<IEnumerable<Assembly>> assemblyProvider)
    {
        if (assemblyProvider == null) throw new ArgumentNullException(nameof(assemblyProvider));

        Assembly[] assemblies = assemblyProvider().ToArray();
        commandMetadataCollection.LoadFrom(assemblies);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFrom(params Assembly[] assemblies)
    {
        commandMetadataCollection.LoadFrom(assemblies);

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
        action(serviceCollection);

        return this;
    }

    public Application Build()
    {
        IServiceProvider serviceProvider = FinalizeContainerSetup();
        return serviceProvider.GetService<Application>();
    }

    private IServiceProvider FinalizeContainerSetup()
    {
        if (!isCommandParserConfigured)
            serviceCollection.AddTransient(typeof(ICommandParser), typeof(CommandParser));

        commandMetadataCollection.Freeze();

        foreach (Type type in commandMetadataCollection.GetCommandTypes())
            serviceCollection.AddTransient(type);

        foreach (Type type in commandMetadataCollection.GetViewTypes())
            serviceCollection.AddTransient(type);

        return serviceCollection.BuildServiceProvider();
    }
}