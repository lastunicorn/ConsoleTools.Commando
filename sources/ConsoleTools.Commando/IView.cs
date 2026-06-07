namespace DustInTheWind.ConsoleTools.Commando;

/// <summary>
/// Represents a view that displays the data from a view model.
/// </summary>
/// <typeparam name="TViewModel">The data to be displayed.</typeparam>
public interface IView<in TViewModel>
{
    void Display(TViewModel viewModel);
}