using DustInTheWind.ConsoleTools.Commando.MetadataModel;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

public class CommandParameterInfo
{
    public string Name { get; }

    public char ShortName { get; }

    public int? Order { get; }

    [Obsolete("Replaced by the IsMandatory property.")]
    public bool IsOptional { get; }

    public bool IsMandatory { get; }

    public string DisplayName { get; }

    public string Description { get; }

    public Type ParameterType { get; }

    public CommandParameterInfo(ParameterMetadata parameterMetadata)
    {
        Name = parameterMetadata.Name;
        ShortName = parameterMetadata.ShortName;
        DisplayName = parameterMetadata.DisplayName;
        IsOptional = parameterMetadata.IsOptional;
        IsMandatory = parameterMetadata.IsMandatory;
        Order = parameterMetadata.Order;
        Description = parameterMetadata.Description;
        ParameterType = parameterMetadata.ParameterType;
    }
}