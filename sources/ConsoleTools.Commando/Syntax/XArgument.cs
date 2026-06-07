namespace DustInTheWind.ConsoleTools.Commando.Syntax;

public record XArgument
{
    public string Name { get; }

    public string Value { get; }

    public bool IsNamed => Name != null;

    public bool IsUnnamed => Name == null;

    public XArgument(string name, string value)
    {
        if (name == null && value == null)
            throw new ArgumentException("Both name and value cannot be null in the same time.", nameof(name));

        Name = name;
        Value = value;
    }
}