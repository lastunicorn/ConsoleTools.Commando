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

namespace DustInTheWind.ConsoleTools.Commando;

/// <summary>
/// Represents a console command that handles a specific request from the user.
/// The correct command to be executed is identified based on the command line arguments provided
/// by the user in the console.
/// </summary>
public interface IConsoleCommand<TResponse>
{
    /// <summary>
    /// When implemented by an inheritor, it executes asynchronously the actions needed for handling
    /// the user's request.
    /// </summary>
    Task<TResponse> Execute();
}