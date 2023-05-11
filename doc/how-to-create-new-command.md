# How to create a new command?

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

## 2) Decorate the class with `CommandAttribute`

The `CommandAttribute` is optional, but, if provided, it allows to specify some information about the command:

- **name** - This is the text used in the console by the user to execute the command.
  - If no name is provided or the entire `CommandAttribute` is missing, than the name of the command is considered to be the name of the class. The trailing "Command" text from the name, if present, it is ignored. (Similar to ASP.NET)
- **short description** - This will be displayed to the user in the help.

```c#
[Command("read", ShortDescription = "Reads the content of the specified file.")]
internal class ReadCommand : ICommand
{
    ...
}
```

Other optional parameters may be provided:

- **order**

1. sfsfd

2. sdf

3. dsf