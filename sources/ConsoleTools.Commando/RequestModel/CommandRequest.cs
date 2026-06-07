using DustInTheWind.ConsoleTools.Commando.Metadata;

namespace DustInTheWind.ConsoleTools.Commando.RequestModel;

public class CommandRequest
{
    private readonly List<CommandArgument> arguments = new();
    private List<CommandArgument> unusedArguments = new();

    public string[] UnderlyingArgs { get; init; }

    public string CommandName { get; set; }

    /// <summary>
    /// Gets the named arguments, also called flags, e.g. --option=value or -o value.
    /// </summary>
    public IReadOnlyCollection<CommandArgument> NamedArguments => arguments
        .Where(x => x.Name != null)
        .ToList();

    /// <summary>
    /// Gets the arguments that do not have a name, e.g. value.
    /// </summary>
    public IReadOnlyCollection<CommandArgument> UnnamedArguments => arguments
        .Where(x => x.Name == null)
        .ToList();

    public bool IsEmpty => CommandName == null && arguments.Count == 0;

    public bool HasUnusedArguments => unusedArguments.Count > 0;

    public void AddParameter(CommandArgument commandArgument)
    {
        if (commandArgument == null) throw new ArgumentNullException(nameof(commandArgument));

        arguments.Add(commandArgument);
    }

    public void Reset()
    {
        unusedArguments = arguments.ToList();
    }

    public CommandArgument GetOptionAndMarkAsUsed(ParameterMetadata parameterMetadata)
    {
        if (parameterMetadata.Name != null)
        {
            CommandArgument commandArgument = arguments
                .Where(x => x.Name != null)
                .FirstOrDefault(x => x.Name == parameterMetadata.Name);

            if (commandArgument != null)
            {
                unusedArguments.Remove(commandArgument);
                return commandArgument;
            }
        }

        if (parameterMetadata.ShortName != 0)
        {
            CommandArgument commandArgument = arguments
                .Where(x => x.Name != null)
                .FirstOrDefault(x => x.Name == parameterMetadata.ShortName.ToString());

            if (commandArgument != null)
            {
                unusedArguments.Remove(commandArgument);
                return commandArgument;
            }
        }

        return null;
    }

    public CommandArgument GetOperandAndMarkAsUsed(ParameterMetadata parameterMetadata)
    {
        if (parameterMetadata.Order != null)
        {
            int index = parameterMetadata.Order.Value - 1;

            if (index >= 0)
            {
                CommandArgument commandArgument = arguments
                    .Where(x => x.Name == null)
                    .Skip(index)
                    .FirstOrDefault();

                if (commandArgument != null)
                {
                    unusedArguments.Remove(commandArgument);
                    return commandArgument;
                }
            }
        }

        return null;
    }

    public IEnumerable<CommandArgument> EnumerateUnusedOptions()
    {
        return unusedArguments
            .Where(x => x.Name != null);
    }

    public IEnumerable<string> EnumerateUnusedOperands()
    {
        return unusedArguments
            .Where(x => x.Name == null)
            .Select(x => x.Value);
    }
}