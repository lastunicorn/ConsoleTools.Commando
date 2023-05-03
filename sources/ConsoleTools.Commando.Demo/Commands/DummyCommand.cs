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

using System;
using System.Threading.Tasks;

namespace DustInTheWind.ConsoleTools.Commando.Demo.Commands;

[Command("dummy", ShortDescription = "A dummy command that shows how to use Commando.A dummy command that shows how to use Commando.A dummy command that shows how to use Commando.")]
public class DummyCommand : ICommand
{
    [CommandParameter(Name = "text", ShortName = 't')]
    public string Text { get; set; }

    [CommandParameter(Name = "flag", ShortName = 'f', IsOptional = true)]
    public bool Flag { get; set; }

    [CommandParameter(Name = "integer", ShortName = 'i', IsOptional = true)]
    public int IntegerNumber { get; set; }

    [CommandParameter(Name = "real", ShortName = 'r', IsOptional = true)]
    public float RealNumber { get; set; }

    [CommandParameter(Name = "char", ShortName = 'c', IsOptional = true)]
    public char Character { get; set; }

    [CommandParameter(Order = 1, IsOptional = true)]
    public string FilePath { get; set; }

    public DummyView View { get; set; }

    public DummyCommand(DummyView dummyView)
    {
        View = dummyView ?? throw new ArgumentNullException(nameof(dummyView));
    }

    public Task Execute()
    {
        View.WriteTitle("This is the Dummy Command.");

        View.WithIndentation("Initial values:", () =>
        {
            View.WriteValue("Text", Text);
            View.WriteValue("Flag", Flag);
            View.WriteValue("Integer Number", IntegerNumber);
            View.WriteValue("Real Number", RealNumber);
            View.WriteValue("Character", Character);
            View.WriteValue("File Path", FilePath);
        });
        View.WriteLine();

        Text = "This is a new text.";
        Flag = false;
        IntegerNumber = 184603;
        RealNumber = 3.141592653f;
        Character = '╢';

        return Task.CompletedTask;
    }
}