namespace DustInTheWind.ConsoleTools.Commando;

public class NamedParameterAttribute : CommandParameterAttribute
{
    public string Name { get; set; }

    public char ShortName { get; set; }

    public NamedParameterAttribute(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("The name is mandatory.", nameof(name));

        Name = name;
    }
}