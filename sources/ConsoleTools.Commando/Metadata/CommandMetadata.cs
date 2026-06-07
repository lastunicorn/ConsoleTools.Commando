using System.Reflection;

namespace DustInTheWind.ConsoleTools.Commando.Metadata;

/// <summary>
/// Provides metadata about a command, like its name, description, etc.
/// </summary>
public class CommandMetadata
{
    private CommandAttribute commandAttribute;
    private CommandOrderAttribute commandOrderAttribute;
    private readonly List<string> descriptionLines;

    public string Name { get; }

    public IReadOnlyList<string> DescriptionLines => descriptionLines;

    public Type Type { get; }

    public CommandKind CommandKind { get; }

    public int Order => commandOrderAttribute?.Order ?? int.MaxValue;

    public bool IsEnabled => commandAttribute?.Enabled ?? true;

    public bool IsHelpCommand { get; private set; }

    public CommandMetadata(Type commandType)
    {
        Type = commandType ?? throw new ArgumentNullException(nameof(commandType));

        CommandKind = ComputeCommandKind(commandType);

        if (CommandKind != CommandKind.None)
        {
            RetrieveAttributes();
            Name = ComputeCommandName();
            descriptionLines = ComputeDescription();
        }
    }

    private static CommandKind ComputeCommandKind(Type type)
    {
        if (!type.IsClass || type.IsAbstract)
            return CommandKind.None;

        bool isNonGenericCommand = typeof(IConsoleCommand).IsAssignableFrom(type);

        if (isNonGenericCommand)
            return CommandKind.WithoutResult;

        bool isGenericCommand = type.GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConsoleCommand<>));

        if (isGenericCommand)
            return CommandKind.WithResult;

        return CommandKind.None;
    }

    private void RetrieveAttributes()
    {
        commandAttribute = Type.GetCustomAttributes(typeof(CommandAttribute), false)
            .Cast<CommandAttribute>()
            .SingleOrDefault();

        IsHelpCommand = commandAttribute is HelpCommandAttribute;

        commandOrderAttribute = Type.GetCustomAttributes(typeof(CommandOrderAttribute), false)
            .Cast<CommandOrderAttribute>()
            .FirstOrDefault();
    }

    private string ComputeCommandName()
    {
        if (commandAttribute is NamedCommandAttribute namedCommandAttribute)
            return namedCommandAttribute.Name;

        return null;
    }

    private List<string> ComputeDescription()
    {
        List<string> lines = new();

        if (commandAttribute != null && !string.IsNullOrEmpty(commandAttribute.Description))
            lines.Add(commandAttribute.Description);

        return lines;
    }

    public IEnumerable<ParameterMetadata> EnumerateParameters()
    {
        return Type.GetProperties()
            .Select(x =>
            {
                CommandParameterAttribute customAttribute = x.GetCustomAttributes<CommandParameterAttribute>()
                    .SingleOrDefault();

                return customAttribute == null
                    ? null
                    : new ParameterMetadata(x, customAttribute);
            })
            .Where(x => x != null)
            .ToArray();
    }

    public IEnumerable<ParameterMetadata> EnumerateNamedParameters()
    {
        return EnumerateParameters()
            .Where(x => x.Name != null || x.ShortName != 0);
    }

    public override string ToString()
    {
        return Name ?? "anonymous command " + Order;
    }
}