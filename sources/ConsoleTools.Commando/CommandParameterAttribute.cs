namespace DustInTheWind.ConsoleTools.Commando;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CommandParameterAttribute : Attribute
{
    private bool isMandatory = true;

    [Obsolete("Replaced by the IsMandatory property.")]
    public bool IsOptional { get; set; }

    public bool IsMandatory
    {
        get => isMandatory;
        set
        {
            isMandatory = value;
            IsMandatorySelected = true;
        }
    }

    internal bool IsMandatorySelected { get; private set; }

    public string Description { get; set; }
}