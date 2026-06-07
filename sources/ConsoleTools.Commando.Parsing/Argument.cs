namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class Argument
{
    public string Name { get; init; }

    public string Value { get; init; }

    public bool IsNamedArgument => Name != null;

    public bool IsAnonymousArgument => Name == null;

    public bool IsForcedToBeAnonymous { get; init; }

    public override string ToString()
    {
        string argumentType = string.Empty;

        if (IsNamedArgument)
            argumentType += "Named";

        if (IsAnonymousArgument)
            argumentType += "Anonymous";

        if (Name != null && Value != null)
            return $"{Name} = {Value} []";

        if (Name != null)
            return $"{Name} [{argumentType}]";

        if (Value != null)
            return $"{Value} [{argumentType}]";

        return $"null [{argumentType}]";
    }
}