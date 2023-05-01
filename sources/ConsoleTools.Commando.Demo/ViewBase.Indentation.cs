using System;

namespace DustInTheWind.ConsoleTools.Commando.Demo;

public partial class ViewBase<T>
{
    private int indentLevel;

    public void IncreaseIndentation()
    {
        if (indentLevel < int.MaxValue)
            indentLevel++;
    }

    public void DecreaseIndentation()
    {
        if (indentLevel > 0)
            indentLevel--;
    }

    public void WithIndentation(string title, Action action)
    {
        WriteInfo(title);
        WithIndentation(action);
    }

    public T WithIndentation<T>(string title, Func<T> action)
    {
        WriteInfo(title);
        return WithIndentation(action);
    }

    public void WithIndentation(Action action)
    {
        if (action == null)
            return;

        IncreaseIndentation();

        try
        {
            action();
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }
        finally
        {
            DecreaseIndentation();
        }
    }

    public T WithIndentation<T>(Func<T> action)
    {
        if (action == null)
            return default;

        IncreaseIndentation();

        try
        {
            return action();
        }
        catch (Exception ex)
        {
            WriteError(ex);
            return default;
        }
        finally
        {
            DecreaseIndentation();
        }
    }

    private void DisplayIndentation()
    {
        string indentationText = new(' ', indentLevel * 4);
        Console.Write(indentationText);
    }
}