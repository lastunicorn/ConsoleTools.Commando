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

namespace DustInTheWind.ConsoleTools.Commando.Demo.Autofac.DependencyInjection.Commands;

[NamedCommand("read", Description = "Display the content of a text file.")]
public class ReadFileCommand : ConsoleCommandBase
{
    [AnonymousParameter(Order = 1, Description = "The path to the file that should be displayed.")]
    public string FilePath { get; set; }

    public ReadFileCommand(EnhancedConsole console)
        : base(console)
    {
    }

    public override Task Execute()
    {
        Console.WriteTitle("Reading a text file");
        Console.WriteValue("File", FilePath);

        string content = File.ReadAllText(FilePath);
        Console.WriteValueBelowName("Content", content);

        return Task.CompletedTask;
    }
}