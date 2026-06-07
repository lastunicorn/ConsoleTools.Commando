using DustInTheWind.ConsoleTools.Commando.Metadata;

namespace DustInTheWind.ConsoleTools.Commando.Syntax;

public class XCommand
{
    private readonly List<XArgument> arguments = new();
    private List<XArgument> unusedArguments = new();

    public string[] UnderlyingArgs { get; init; }

    public string Name { get; set; }
    
    public string Action { get; set; }

    /// <summary>
    /// Gets the named arguments.
    /// They are called options when the value is explicitly specified (e.g. --option=value or -o value)
    /// or flags when the value is not specified (e.g. --flag or -f).
    /// </summary>
    public IReadOnlyCollection<XArgument> NamedArguments => arguments
        .Where(x => x.Name != null)
        .ToList();

    /// <summary>
    /// Gets the arguments that do not have a name, e.g. value.
    /// </summary>
    public IReadOnlyCollection<XArgument> UnnamedArguments => arguments
        .Where(x => x.Name == null)
        .ToList();

    public bool IsEmpty => Name == null && arguments.Count == 0;

    public bool HasUnusedArguments => unusedArguments.Count > 0;

    public void AddParameter(XArgument xArgument)
    {
        if (xArgument == null) throw new ArgumentNullException(nameof(xArgument));

        arguments.Add(xArgument);
    }

    public void Reset()
    {
        unusedArguments = arguments.ToList();
    }

    public XArgument GetOptionAndMarkAsUsed(ParameterMetadata parameterMetadata)
    {
        if (parameterMetadata.Name != null)
        {
            XArgument xArgument = arguments
                .Where(x => x.Name != null)
                .FirstOrDefault(x => x.Name == parameterMetadata.Name);

            if (xArgument != null)
            {
                unusedArguments.Remove(xArgument);
                return xArgument;
            }
        }

        if (parameterMetadata.ShortName != 0)
        {
            XArgument xArgument = arguments
                .Where(x => x.Name != null)
                .FirstOrDefault(x => x.Name == parameterMetadata.ShortName.ToString());

            if (xArgument != null)
            {
                unusedArguments.Remove(xArgument);
                return xArgument;
            }
        }

        return null;
    }

    public XArgument GetOperandAndMarkAsUsed(ParameterMetadata parameterMetadata)
    {
        if (parameterMetadata.Order != null)
        {
            int index = parameterMetadata.Order.Value - 1;

            if (index >= 0)
            {
                XArgument xArgument = arguments
                    .Where(x => x.Name == null)
                    .Skip(index)
                    .FirstOrDefault();

                if (xArgument != null)
                {
                    unusedArguments.Remove(xArgument);
                    return xArgument;
                }
            }
        }

        return null;
    }

    public IEnumerable<XArgument> EnumerateUnusedOptions()
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