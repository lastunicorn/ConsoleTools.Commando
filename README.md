# Console Tools Commando

This is a presentation layer framework that helps you create a CLI (command line interface) applications.

## Quick Start (with Autofac)

### 1) Include the NuGet package:

- `ConsoleTools.Commando.Setup.Autofac`
- Note:
  - The `ConsoleTools.Commando` package will be automatically included.

### 2) Create your command

#### Command Class

The public properties are automatically populated by the router with arguments from the user's CLI command.

When executed, the command returns a View Model instance.

```c#
[NamedCommand("read", Description = "Display the content of a text file.")]
internal class ReadFileCommand : IConsoleCommand<ReadFileViewModel>
{
    [NamedParameter("file", ShortName = 'f', Description = "The full path of the file.")]
    public string FilePath { get; set; }
    
	public Task<ReadFileViewModel> Execute()
	{
		...
	}
}
```

#### View Model Class

The View Model class is a POCO containing the data to be displayed by the View.

```c#
internal class ReadFileViewModel
{
    public string FilePath { get; set; }

    public string Content { get; set; }
}
```

#### View Class

The View Class must implement the `IView<TViewModel>` interface.

Alternatively, it may implement the base class `ViewBase<TViewModel>` which provide a number of helper methods to help display data in the console.

```c#
internal class ReadFileView : ViewBase<ReadFileViewModel>
{
    public override void Display(ReadFileViewModel viewModel)
    {
        ...
    }
}
```

### 3) Run the `Application`

```c#
Application application = ApplicationBuilder.Create()
    .RegisterCommandsFrom(typeof(ReadFileCommand).Assembly) // Provide the assembly containing your commands.
    .Build();

await application.RunAsync(args);
```



