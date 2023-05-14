# How to use? (Builder Approach)

Commando offers support for three dependency injection frameworks:

- [Autofac](https://autofac.org/)
- [Ninject](http://www.ninject.org/)
- Microsoft Dependency Injection

## Step 1 - Include the nuget package

Reference the nuget package corresponding to the dependency injection framework that you want to use.

- `ConsoleTools.Commando.Builder.Autofac`
- `ConsoleTools.Commando.Builder.Ninject`
- `ConsoleTools.Commando.Builder.Microsoft`

## Step 2 - Build and run the `Application`.

In the `Main` method, use an `ApplicationBuilder`:

```csharp
Application application = ApplicationBuilder.Create()
    .RegisterCommandsFrom(typeof(DummyCommand).Assembly) // Provide here the assembly containing your commands.
    .Build();

await application.RunAsync(args);
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



