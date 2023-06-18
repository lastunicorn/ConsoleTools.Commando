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

using System.Text;

namespace DustInTheWind.ConsoleTools.Commando.Demo.Autofac.Builder.Commands.ReadFile;

[NamedCommand("read", Description = "Display the content of a text file.")]
public class ReadFileCommand : IConsoleCommand<ReadFileViewModel>
{
    [AnonymousParameter(Order = 1, DisplayName = "file path", Description = "The path to the file that should be displayed.")]
    public string FilePath { get; set; }

    [NamedParameter("encoding", ShortName = 'e', IsOptional = true, Description = "The text encoding to be used when reading the file.")]
    public Encoding Encoding { get; set; }

    public Task<ReadFileViewModel> Execute()
    {
        string content = Encoding == null
            ? File.ReadAllText(FilePath)
            : File.ReadAllText(FilePath, Encoding);

        ReadFileViewModel viewModel = new()
        {
            FilePath = FilePath,
            Content = content
        };

        return Task.FromResult(viewModel);
    }
}