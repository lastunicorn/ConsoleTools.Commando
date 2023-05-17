﻿// ConsoleTools.Commando
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

namespace DustInTheWind.ConsoleTools.Commando.Demo.Autofac.Builder.Commands;

[DefaultCommand(Order = 100, Description = "Default command to be executed when no command name is specified.")]
public class DefaultCommand : CommandBase
{
    [NamedParameter("text")]
    public string Text { get; set; }

    public DefaultCommand(EnhancedConsole enhancedConsole)
        : base(enhancedConsole)
    {
    }

    public override Task Execute()
    {
        Console.WriteTitle("Default command");
        Console.WriteValue("Text", Text);
        
        return Task.CompletedTask;
    }
}