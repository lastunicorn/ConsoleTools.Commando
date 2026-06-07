namespace DustInTheWind.ConsoleTools.Commando;

[AttributeUsage(AttributeTargets.Class)]
public class CommandOrderAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the order in which this command will appear in the help list.
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandOrderAttribute"/> class
    /// with an integer representing the order in which this command will appear in the help list.
    /// </summary>
    /// <param name="order">The order in which this command will appear in the help list.</param>
    public CommandOrderAttribute(int order)
    {
        Order = order;
    }
}