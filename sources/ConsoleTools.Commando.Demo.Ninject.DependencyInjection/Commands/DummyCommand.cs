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

namespace DustInTheWind.ConsoleTools.Commando.Demo.Ninject.DependencyInjection.Commands;

[NamedCommand("dummy", Description = "A dummy command that shows how to create parameters of different types.")]
public class DummyCommand : CommandBase
{
    [NamedParameter("text", ShortName = 't')]
    public string Text { get; set; }

    [NamedParameter("flag", ShortName = 'f', IsOptional = true)]
    public bool Flag { get; set; }

    [NamedParameter("integer", ShortName = 'i', IsOptional = true)]
    public int IntegerNumber { get; set; }

    [NamedParameter("real", ShortName = 'r', IsOptional = true)]
    public float RealNumber { get; set; }

    [NamedParameter("char", ShortName = 'c', IsOptional = true)]
    public char Character { get; set; }

    [AnonymousParameter(Order = 1, IsOptional = true)]
    public string FilePath { get; set; }

    public DummyCommand(EnhancedConsole console)
        : base(console)
    {
    }

    public override Task Execute()
    {
        Console.WriteTitle("This is the Dummy Command.");

        Console.WithIndentation("Initial values:", () =>
        {
            Console.WriteValue("Text", Text);
            Console.WriteValue("Flag", Flag);
            Console.WriteValue("Integer Number", IntegerNumber);
            Console.WriteValue("Real Number", RealNumber);
            Console.WriteValue("Character", Character);
            Console.WriteValue("File Path", FilePath);
        });
        Console.WriteLine();

        Text = "This is a new text.";
        Flag = false;
        IntegerNumber = 184603;
        RealNumber = 3.141592653f;
        Character = '╢';

        return Task.CompletedTask;
    }
}