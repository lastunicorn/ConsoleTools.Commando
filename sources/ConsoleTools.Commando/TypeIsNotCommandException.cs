﻿// ConsoleTools.Commando
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

public class TypeIsNotCommandException : Exception
{
    public TypeIsNotCommandException(Type type)
        : base(BuildMessage(type))
    {
    }

    private static string BuildMessage(Type type)
    {
        string typeFullName = type.FullName;
        string commandType1FullName = typeof(IConsoleCommand).FullName;
        string commandType2FullName = typeof(IConsoleCommand<>).FullName;

        return $"Type {typeFullName} does not represent a command. A command must implement the {commandType1FullName} or {commandType2FullName} interface.";
    }
}