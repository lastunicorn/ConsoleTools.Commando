using DustInTheWind.ConsoleTools.Commando.MetadataModel;

namespace DustInTheWind.ConsoleTools.Commando;

public interface ICommandFactory
{
    object Create(CommandMetadata commandMetadata);

    object CreateView(Type viewType);
}