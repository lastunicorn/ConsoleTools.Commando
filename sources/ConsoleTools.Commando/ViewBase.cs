namespace DustInTheWind.ConsoleTools.Commando;

public abstract class ViewBase<TViewModel> : EnhancedConsole, IView<TViewModel>
{
    public abstract void Display(TViewModel viewModel);
}