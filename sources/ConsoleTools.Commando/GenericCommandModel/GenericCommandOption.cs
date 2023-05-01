using System;

namespace DustInTheWind.ConsoleTools.Commando.GenericCommandModel;

public class GenericCommandOption
{
    public string Name { get; }

    public string Value { get; }

    public GenericCommandOption(string name, string value)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return Value == null
            ? $"{Name}"
            : $"{Name} = {Value}";
    }
}