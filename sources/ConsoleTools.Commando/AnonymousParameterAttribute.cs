namespace DustInTheWind.ConsoleTools.Commando;

public class AnonymousParameterAttribute : CommandParameterAttribute
{
    public string DisplayName { get; set; }

    public int Order { get; set; }
}