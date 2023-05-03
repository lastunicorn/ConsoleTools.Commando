using System;

namespace DustInTheWind.ConsoleTools.Commando.Demo;

public partial class ViewBase<TCommand>
{
    public void WriteValue(string name, object value)
    {
        DisplayIndentation();
        DisplayValueInternal(name, value);
    }

    public void WriteError(Exception exception)
    {
        WriteLineWithColor(ErrorColor, exception.ToString());
    }

    public void WriteError(string message)
    {
        DisplayIndentation();
        WriteLineWithColor(ErrorColor, message);
    }

    public void WriteWarning(string message)
    {
        DisplayIndentation();
        WriteLineWithColor(WarningColor, message);
    }

    public void WriteSuccess(string message)
    {
        DisplayIndentation();
        WriteLineWithColor(SuccessColor, message);
    }

    public void WriteInfo(string message)
    {
        DisplayIndentation();
        WriteLineWithColor(ConsoleColor.Gray, message);
    }
}