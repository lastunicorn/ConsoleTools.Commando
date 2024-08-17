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

namespace DustInTheWind.ConsoleTools.Commando.Demo.Microsoft.DependencyInjection.Commands.Dummy;

[NamedCommand("dummy", Description = "A dummy command that shows how to create parameters of different types.")]
internal class DummyCommand : IConsoleCommand<DummyViewModel>
{
    [NamedParameter("text", ShortName = 't', IsMandatory = false, Description = "A simple text.")]
    public string Text { get; set; }

    [NamedParameter("flag", ShortName = 'f', IsMandatory = false, Description = "An optional flag. Default value = false. If it is provided without a value, the value is considered true.")]
    public bool Flag { get; set; }

    [NamedParameter("flag2", ShortName = 'g', IsMandatory = false, Description = "An optional flag. Default value = false. If it is provided without a value, the value is considered true.")]
    public bool Flag2 { get; set; }

    [NamedParameter("integer", ShortName = 'i', IsMandatory = false, Description = "An optional integer number.")]
    public int IntegerNumber { get; set; }

    [NamedParameter("real", ShortName = 'r', IsMandatory = false, Description = "An optional real number.")]
    public float RealNumber { get; set; }

    [NamedParameter("char", ShortName = 'c', IsMandatory = false, Description = "An optional character. It accepts a single character as value.")]
    public char Character { get; set; }

    [AnonymousParameter(Order = 1, IsMandatory = false, Description = "An anonymous text. It is identified based on its index in the list of arguments.")]
    public string Param1 { get; set; }

    [AnonymousParameter(Order = 2, IsMandatory = false, Description = "An anonymous number. It is identified based on its index in the list of arguments.")]
    public int? Param2 { get; set; }

    public Task<DummyViewModel> Execute()
    {
        DummyViewModel result = new()
        {
            Text = Text,
            Flag = Flag,
            Flag2 = Flag2,
            IntegerNumber = IntegerNumber,
            RealNumber = RealNumber,
            Character = Character,
            Param1 = Param1,
            Param2 = Param2
        };

        return Task.FromResult(result);
    }
}