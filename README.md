# Console Tools Commando

This is an MVVM presentation layer framework that helps you to create a CLI (command line interface).

## How to use (with Autofac)

1. Include the nuget package:

   - `ConsoleTools.Commando.DependencyInjection.Autofac`

2. Build and run the `CommandoHost`.

   ```csharp
   CommandoHost host = CommandoHost.CreateBuilder()
       .RegisterCommandsFrom(typeof(DummyCommand).Assembly) // Provide here the assembly containing your commands.
       .Build();
   
   await host.RunAsync(args);
   ```

3. Create your commands.

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


> **Notes**
>
> 1. The `ConsoleTools.Commando.DependencyInjection.Autofac` package will automatically include:
>
>    - `Autofac`
>
>    - `ConsoleTools.Commando`
>
> 2. The `CommandoHost` approach is available only from version 2.0.0. For the older versions, please see the dependency injection demo projects from the repository.

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

