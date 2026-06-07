using DustInTheWind.ConsoleTools.Commando.MetadataModel;

namespace DustInTheWind.ConsoleTools.Commando.RequestModel;

public class CommandRequest
{
    private readonly List<CommandArgument> arguments = new();
    private List<CommandArgument> unusedArguments = new();

    public string[] UnderlyingArgs { get; init; }

    public string CommandName { get; set; }

    public IReadOnlyCollection<CommandArgument> Options => arguments
        .Where(x => x.Name != null)
        .ToList();

    public IReadOnlyCollection<CommandArgument> Operands => arguments
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