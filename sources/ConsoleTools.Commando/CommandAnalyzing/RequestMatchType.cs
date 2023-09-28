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

namespace DustInTheWind.ConsoleTools.Commando.CommandAnalyzing;

internal enum RequestMatchType
{
    /// <summary>
    /// No command is matching the request.
    /// </summary>
    NoMatch = 0,

    /// <summary>
    /// The command has optional parameters that are not matched by the provided arguments OR
    /// there are arguments from the request that remain unused.
    /// </summary>
    Partial,

    /// <summary>
    /// The command has all the parameters matched by the provided arguments and no arguments from th request remain unused.
    /// </summary>
    Full,

    /// <summary>
    /// Multiple commands fully match the request or, if there is no full match, multiple commands partially match the request.
    /// </summary>
    Multiple,

    /// <summary>
    /// Only the help command matches the request.
    /// </summary>
    Help,

    /// <summary>
    /// The command name matches but not the parameters.
    /// </summary>
    OnlyName
}