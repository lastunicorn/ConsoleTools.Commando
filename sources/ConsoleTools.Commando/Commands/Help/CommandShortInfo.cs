using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Controls;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

public class CommandShortInfo
{
    public string Name { get; }

    public MultilineText Description { get; }

    public CommandShortInfo(CommandMetadata commandMetadata)
    {
        Name = commandMetadata.Name;
        Description = commandMetadata.DescriptionLines.ToList();
    }
}