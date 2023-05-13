# How to use? (Host Approach)

Commando offers support for three dependency injection frameworks:

- [Autofac](https://autofac.org/)
- [Ninject](http://www.ninject.org/)
- Microsoft Dependency Injection

## Step 1 - Include the nuget package

Reference the nuget package corresponding to the dependency injection framework that you want to use.

- `ConsoleTools.Commando.Hosting.Autofac`
- `ConsoleTools.Commando.Hosting.Ninject`
- `ConsoleTools.Commando.Hosting.Microsoft`

## Step 2 - Build and run the `CommandoHost`.

In the `Main` method:

```csharp
CommandoHost host = CommandoHost.CreateBuilder()
    .RegisterCommandsFrom(typeof(DummyCommand).Assembly) // Provide here the assembly containing your commands.
    .Build();

await host.RunAsync(args);
```

## Step 3 - Create your commands.

```c#
[NamedCommand("read", Description = "Display the content of a text file.")]
internal class ReadCommand : ICommand
{
    [NamedParameter("file", ShortName = 'f', Description = "The full path of the file.")]
    public string FilePath { get; set; }
    
	public Task Execute()
	{
		...
	}
}
```



