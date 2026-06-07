namespace DustInTheWind.ConsoleTools.Commando.RequestModel;

public record CommandArgument
{
    public string Name { get; }

    public string Value { get; }

    public CommandArgument(string name, string value)
    {
        if (name == null && value == null)
            throw new ArgumentException("Both name and value cannot be null in the same time.", nameof(name));

        Name = name;
        Value = value;
    }
}