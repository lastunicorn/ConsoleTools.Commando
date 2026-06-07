// ConsoleTools.Commando
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
using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Parsing;
using DustInTheWind.ConsoleTools.Commando.Routing;
using DustInTheWind.ConsoleTools.Commando.Syntax;
using Ninject;

namespace DustInTheWind.ConsoleTools.Commando.Setup.Ninject;

public class ApplicationBuilder
{
    private readonly IKernel kernel;
    private readonly MetadataContext metadataContext;
    private bool isCommandParserConfigured;
    private EventHandler<UnhandledApplicationExceptionEventArgs> unhandledExceptionHandler;

    private ApplicationBuilder()
    {
        kernel = new StandardKernel();
        metadataContext = new MetadataContext();

        ConfigureDefaultServices();
        LoadDefaultCommands();
    }

    public static ApplicationBuilder Create()
    {
        return new ApplicationBuilder();
    }

    private void ConfigureDefaultServices()
    {
        kernel.Bind<ICommandFactory>().To<CommandFactory>();
        kernel.Bind<MetadataContext>().ToConstant(metadataContext).InSingletonScope();
        kernel.Bind<Application>().ToSelf().InSingletonScope();
    }

    private void LoadDefaultCommands()
    {
        metadataContext.LoadFromAssemblyContaining<MetadataContext>();
    }

    public ApplicationBuilder RegisterCommandsFrom(Func<Assembly> assemblyProvider)
    {
        if (assemblyProvider == null) throw new ArgumentNullException(nameof(assemblyProvider));

        Assembly assembly = assemblyProvider();
        metadataContext.LoadFrom(assembly);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFrom(Func<IEnumerable<Assembly>> assemblyProvider)
    {
        if (assemblyProvider == null) throw new ArgumentNullException(nameof(assemblyProvider));

        Assembly[] assemblies = assemblyProvider().ToArray();
        metadataContext.LoadFrom(assemblies);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFrom(params Assembly[] assemblies)
    {
        metadataContext.LoadFrom(assemblies);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFromAssemblyContaining(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        metadataContext.LoadFrom(type.Assembly);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFromCurrentAssembly()
    {
        Assembly assembly = Assembly.GetCallingAssembly();
        metadataContext.LoadFrom(assembly);

        return this;
    }

    public ApplicationBuilder RegisterCommandsFromEntryAssembly()
    {
        Assembly assembly = Assembly.GetEntryAssembly();
        metadataContext.LoadFrom(assembly);

        return this;
    }

    public ApplicationBuilder UseCommandParser(Type commandParserType)
    {
        if (commandParserType == null) throw new ArgumentNullException(nameof(commandParserType));

        bool typeIsCommandParser = typeof(ICliSyntax).IsAssignableFrom(commandParserType);

        if (!typeIsCommandParser)
        {
            string typeFullName = commandParserType.FullName;
            string commandParserTypeFullName = typeof(ICliSyntax).FullName;
            string message = $"Type {typeFullName} does not represent a command parser. A command parser must implement the {commandParserTypeFullName} interface.";

            throw new ArgumentException(message, nameof(commandParserType));
        }

        kernel.Bind<ICliSyntax>().To(commandParserType);

        isCommandParserConfigured = true;

        return this;
    }

    public ApplicationBuilder ConfigureServices(Action<IKernel> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        action(kernel);

        return this;
    }

    public ApplicationBuilder HandleExceptions(EventHandler<UnhandledApplicationExceptionEventArgs> eventHandler)
    {
        unhandledExceptionHandler = eventHandler;

        return this;
    }

    public Application Build()
    {
        IKernel container = FinalizeContainerSetup();
        Application application = container.Get<Application>();

        if (unhandledExceptionHandler != null)
            application.UnhandledApplicationException += unhandledExceptionHandler;

        return application;
    }

    private IKernel FinalizeContainerSetup()
    {
        if (!isCommandParserConfigured)
            kernel.Bind<ICliSyntax>().To<CliSyntax>();

        metadataContext.Freeze();

        foreach (Type type in metadataContext.Commands.GetCommandTypes())
            kernel.Bind(type).ToSelf();

        foreach (Type type in metadataContext.Views.GetViewTypes())
            kernel.Bind(type).ToSelf();

        return kernel;
    }
}