# How to create a new command?

1. **Implement `ICommand`** - A Command must implement the `ICommand` interface.
2. **Add a class attribute** - A class attribute may be used to add more configuration values.
3. **Add command parameters** - Add properties which will be populated with the values provided by the user.

The `Command` attribute is optional and may provide additional information and constraints about the command:

- Command Name
  - Case insensitive.
  - If provided, the command will be executed only if the verb from the generic model matches this value.
- Short Description
  - This value is displayed by the help command.

## 1) Create a class

- Implement the `ICommand` interface.

  ```c#
internal class ReadCommand : ICommand
{
  	public Task Execute()
  	{
  		...
  	}
}
  ```

- It may be `internal`.

## 2) Use a class attribute

The a command attribute is optional, but, if provided, it allows to specify some information about the command. There are two attributes that can be used:

- `NamedCommandAttribute`
- `DefaultCommandAttribute`

### The `NamedCommandAttribute`

```c#
[NamedCommand("read", Description = "Reads the content of the specified file.")]
internal class ReadCommand : ICommand
{
	...
}
```

Allows to specify the following information for the command:

- `Name`
  - This is the text used in the console by the user to execute the command.
  - If no name is provided or the entire `CommandAttribute` is missing, than the name of the command is considered to be the name of the class. The trailing "Command" text from the name, if present, it is ignored. (Similar to ASP.NET)

- `Description`
  - This will be displayed to the user in the help.

- `Order`
  - This is the order in which will displayed by the help command.

  - Default value: 0

- `Enabled`
  - If the command is disabled, it will not be executed and it will not be displayed by the help command.
  - Default value: true


### The `DefaultCommandAttribute`

The command marked with this attribute, will be the one executed when no specific command name is provided.

```c#
[DefaultCommand(Order = 100, Description = "Default command to be executed when no command name is specified.")]
internal class DefaultCommand : ICommand
{
	...
}
```

It has all the properties provided by the `NamedCommandAttribute` except the `Name`

## 3) Add Parameters

When a property of the command class is marked with a parameter attribute, it will automatically be populated with the corresponding argument value provided by the user.

There are two types of parameters:

- `NamedParameterAttribute`
- `AnonymousParameterAttribute`

### The `NamedParameterAttribute`

```c#
[NamedCommand("read", Description = "Display the content of a text file.")]
public class ReadFileCommand : ICommand
{
    [NamedParameter("encoding", ShortName = 'e', IsOptional = true, Description = "Encoding to be used ...")]
    public Encoding Encoding { get; set; }

	public Task Execute()
	{
		...
	}
}
```

### The `AnonymousParameterAttribute`

```c#
[NamedCommand("read", Description = "Display the content of a text file.")]
public class ReadFileCommand : ICommand
{
    [AnonymousParameter(Order = 1, Description = "The path to the file that should be displayed.")]
    public string FilePath { get; set; }
    
	public Task Execute()
	{
		...
	}
}
```

### 
