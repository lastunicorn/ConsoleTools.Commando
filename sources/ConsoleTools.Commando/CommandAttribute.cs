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

using System;

namespace DustInTheWind.ConsoleTools.Commando;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute
{
    /// <summary>
    /// Gets the name of the command.
    /// Ths value is optional, but, if provided, it must not contain spaces.
    /// The command will be executed only if the verb provided by the user matches this value.
    /// </summary>
    public string CommandName { get; }

    /// <summary>
    /// Gets or sets a short description that is displayed by the help command.
    /// </summary>
    public string ShortDescription { get; set; }

    /// <summary>
    /// Gets or sets the order in which this command will appear in the help list.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets a value that specify if the command is allowed to be executed or not.
    /// If the command is disabled, the system behaves as if this command does not even exist.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Creates a new instance of the <see cref="CommandAttribute"/> with
    /// no name.
    /// </summary>
    public CommandAttribute()
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CommandAttribute"/> with
    /// the specified name.
    /// </summary>
    public CommandAttribute(string commandName)
    {
        CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));
    }
}