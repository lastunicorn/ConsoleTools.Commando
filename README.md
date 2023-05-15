# Console Tools Commando

This is an MVVM presentation layer framework that helps you create a CLI (command line interface).

## How to use (with Autofac)

### 1) Include the nuget package:

- `ConsoleTools.Commando.Builder.Autofac`
- Note:
  - The `ConsoleTools.Commando` package will be automatically included.

### 2) Build and run the `Application`.

```c#
Application application = ApplicationBuilder.Create()
    .RegisterCommandsFrom(typeof(ReadCommand).Assembly) // Provide here the assembly containing your commands.
    .Build();

await application.RunAsync(args);
```

### 3) Create your commands.

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

## Discussions and Suggestions

https://github.com/lastunicorn/ConsoleTools.Commando/discussions

I appreciate any opinion or suggestion:

- Did you feel the need for a specific feature?
- Did you like or dislike something?
- Do you have questions?
- etc...

I'm looking forward to hearing from you.

# Donations

If you like my work and want to support me, you can buy me a coffee:

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y62EZ8H)

