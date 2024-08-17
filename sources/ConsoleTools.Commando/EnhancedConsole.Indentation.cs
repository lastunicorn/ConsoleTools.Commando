// ConsoleTools.Commando
// Copyright (C) 2022-2024 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace DustInTheWind.ConsoleTools.Commando;

public partial class EnhancedConsole
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