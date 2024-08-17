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

namespace DustInTheWind.ConsoleTools.Commando.CommandAnalyzing;

internal enum CommandMatchType
{
    /// <summary>
    /// The command does not match the request.
    /// </summary>
    NoMatch = 0,

    /// <summary>
    /// The command has optional parameters that are not matched by the provided arguments or
    /// there are arguments from the request that remain unused.
    /// </summary>
    Partial,

    /// <summary>
    /// The command has all the parameters matched by the provided arguments and
    /// no arguments from th request remain unused.
    /// </summary>
    Full
}