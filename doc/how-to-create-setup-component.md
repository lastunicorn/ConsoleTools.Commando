# How to Create a Setup Component?

As an example you may look into the existing startup components:

- `ConsoleTools.Commando.Autofac.DependencyInjection`
- `ConsoleTools.Commando.Ninject.DependencyInjection` 

## Create an extension method

The setup process is composed of a number of dependencies that need to be registered into the dependency container of choice:

- Command Parser
- Command Router
- Command Factory
- Command Metadata Collection
- Each Command type
- Each View type
- Application

If we use Autofac as dependency container, the code looks like this:

```c#
public static void RegisterCommando(this ContainerBuilder containerBuilder, Type commandParserType, params Assembly[] assemblies)
{
    containerBuilder.RegisterType<CommandRouter>().AsSelf();
    containerBuilder.RegisterType<CommandFactory>().As<ICommandFactory>();
    containerBuilder.RegisterType(commandParserType).As<ICommandParser>();

    CommandMetadataCollection commandMetadataCollection = new();
    Assembly commandoAssembly = typeof(CommandMetadataCollection).Assembly;
    commandMetadataCollection.LoadFrom(commandoAssembly);
    commandMetadataCollection.LoadFrom(assemblies);

    containerBuilder.RegisterInstance(commandMetadataCollection).AsSelf().SingleInstance();

    foreach (Type type in commandMetadataCollection.GetCommandTypes())
        containerBuilder.RegisterType(type).AsSelf();

    foreach (Type type in commandMetadataCollection.GetViewTypes())
        containerBuilder.RegisterType(type).AsSelf();

    containerBuilder.RegisterType<Application>().AsSelf().SingleInstance();
}
```

