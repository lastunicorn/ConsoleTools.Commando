// ConsoleTools.Commando
// Copyright (C) 2022 Dust in the Wind
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

using System.Threading.Tasks;

namespace DustInTheWind.ConsoleTools.Commando.Demo.Commands;

[Command("dummy", ShortDescription = "A dummy command that shows how to use Commando.")]
public class DummyCommand : ICommand
{
    [CommandParameter(Name = "text", ShortName = 't', IsOptional = false)]
    public string DummyText { get; set; }

    public Task Execute()
    {
        DummyText += " - text was updated";

        return Task.CompletedTask;
    }
}