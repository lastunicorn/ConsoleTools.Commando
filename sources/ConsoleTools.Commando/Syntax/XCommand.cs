using DustInTheWind.ConsoleTools.Commando.Metadata;

namespace DustInTheWind.ConsoleTools.Commando.Syntax;

public class XCommand
{
    private readonly List<XArgument> arguments = new();

    public string[] UnderlyingArgs { get; init; }

    public string Name { get; set; }
    
    public string Action { get; set; }
    
    public IReadOnlyCollection<XArgument> Arguments => arguments;

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

    public void AddParameter(XArgument xArgument)
    {
        if (xArgument == null) throw new ArgumentNullException(nameof(xArgument));

        arguments.Add(xArgument);
    }
}