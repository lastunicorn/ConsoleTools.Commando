// ConsoleTools.Commando
// Copyright (C) 2022-2023 Dust in the Wind
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
        WriteLineWithColor(InfoColor, message);
    }

    public void WriteNote(string message)
    {
        DisplayIndentation();
        WriteLineWithColor(NoteColor, message);
    }
}