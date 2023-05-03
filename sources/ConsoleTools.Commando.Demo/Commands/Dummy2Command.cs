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

public class Dummy2Command : ICommand
{
    [CommandParameter(Name = "text", ShortName = 't', Description = "Some useless text.")]
    public string Text { get; set; }

    public DummyView View { get; set; }

    public Dummy2Command(DummyView dummyView)
    {
        View = dummyView ?? throw new ArgumentNullException(nameof(dummyView));
    }

    public Task Execute()
    {
        View.WriteValue("Text", Text);

        return Task.CompletedTask;
    }
}