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

namespace DustInTheWind.ConsoleTools.Commando;

[AttributeUsage(AttributeTargets.Class)]
public class CommandOrderAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the order in which this command will appear in the help list.
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandOrderAttribute"/> class
    /// with an integer representing the order in which this command will appear in the help list.
    /// </summary>
    /// <param name="order">The order in which this command will appear in the help list.</param>
    public CommandOrderAttribute(int order)
    {
        Order = order;
    }
}