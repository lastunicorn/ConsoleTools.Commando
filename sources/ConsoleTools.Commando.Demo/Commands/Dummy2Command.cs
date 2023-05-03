using System;
using System.Threading.Tasks;

namespace DustInTheWind.ConsoleTools.Commando.Demo.Commands;

public class Dummy2Command : ICommand
{
    [CommandParameter(Name = "text", ShortName = 't', Description = "Some useless text.")]
    public string Text { get; set; }

    public DummyView View { get; set; }

    public Dummy2Command(DummyView dummyView)
    {
        View = dummyView ?? throw new ArgumentNullException(nameof(dummyView));
    }

    public Task Execute()
    {
        View.WriteValue("Text", Text);

        return Task.CompletedTask;
    }
}