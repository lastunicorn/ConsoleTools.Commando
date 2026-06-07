namespace DustInTheWind.ConsoleTools.Commando;

[AttributeUsage(AttributeTargets.Class)]
public class NamedCommandAttribute : CommandAttribute
{
    /// <summary>
    /// Gets the name of the command.
    /// This value is optional, and it must not contain spaces.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="NamedCommandAttribute" /> with
    /// the specified name.
    /// </summary>
    public NamedCommandAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}