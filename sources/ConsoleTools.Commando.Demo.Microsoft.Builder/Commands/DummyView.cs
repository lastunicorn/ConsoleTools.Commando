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

namespace DustInTheWind.ConsoleTools.Commando.Demo.Microsoft.Builder.Commands;

public class DummyView : ViewBase<DummyCommand>
{
    public override void Display(DummyCommand command)
    {
        WithIndentation("Same values after command finished execution:", () =>
        {
            WriteValue("Text", command.Text);
            WriteValue("Flag", command.Flag);
            WriteValue("Integer Number", command.IntegerNumber);
            WriteValue("Real Number", command.RealNumber);
            WriteValue("Character", command.Character);
            WriteValue("Param 1", command.Param1);
            WriteValue("Param 2", command.Param2);
        });
    }
}