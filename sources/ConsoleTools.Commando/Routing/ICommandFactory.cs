using DustInTheWind.ConsoleTools.Commando.Metadata;

namespace DustInTheWind.ConsoleTools.Commando.Routing;

public interface ICommandFactory
{
    object Create(CommandMetadata commandMetadata);

    object CreateView(Type viewType);
}