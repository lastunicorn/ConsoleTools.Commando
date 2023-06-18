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

namespace DustInTheWind.ConsoleTools.Commando.Demo.Ninject.Builder.Commands;

[NamedCommand("dummy", Description = "A dummy command that shows how to create parameters of different types.")]
public class DummyCommand : ConsoleCommandBase
{
    [NamedParameter("text", ShortName = 't', IsOptional = true, Description = "A simple text.")]
    public string Text { get; set; }

    [NamedParameter("flag", ShortName = 'f', IsOptional = true, Description = "An optional flag. Default value = false. If it is provided without a value, the value is considered true.")]
    public bool Flag { get; set; }

    [NamedParameter("flag2", ShortName = 'g', IsOptional = true, Description = "An optional flag. Default value = false. If it is provided without a value, the value is considered true.")]
    public bool Flag2 { get; set; }

    [NamedParameter("integer", ShortName = 'i', IsOptional = true, Description = "An optional integer number.")]
    public int IntegerNumber { get; set; }

    [NamedParameter("real", ShortName = 'r', IsOptional = true, Description = "An optional real number.")]
    public float RealNumber { get; set; }

    [NamedParameter("char", ShortName = 'c', IsOptional = true, Description = "An optional character. It accepts a single character as value.")]
    public char Character { get; set; }

    [AnonymousParameter(Order = 1, IsOptional = true, Description = "An anonymous text. It is identified based on its index in the list of arguments.")]
    public string Param1 { get; set; }

    [AnonymousParameter(Order = 2, IsOptional = true, Description = "An anonymous number. It is identified based on its index in the list of arguments.")]
    public int? Param2 { get; set; }

    public DummyCommand(EnhancedConsole console)
        : base(console)
    {
    }

    public override Task Execute()
    {
        Console.WriteTitle("Dummy Command");

        Console.WithIndentation("Initial values:", () =>
        {
            Console.WriteValue("Text", Text);
            Console.WriteValue("Flag", Flag);
            Console.WriteValue("Integer Number", IntegerNumber);
            Console.WriteValue("Real Number", RealNumber);
            Console.WriteValue("Character", Character);
            Console.WriteValue("Param 1", Param1);
            Console.WriteValue("Param 2", Param2);
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