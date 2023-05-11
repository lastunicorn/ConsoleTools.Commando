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

namespace DustInTheWind.ConsoleTools.Commando;

public partial class EnhancedConsole
{
    public void WriteLine()
    {
        Console.WriteLine();
    }

    public void WriteHorizontalLine(int margin = 0)
    {
        for (int i = 0; i < margin; i++)
            Console.WriteLine();

        Console.WriteLine(new string('-', 79));

        for (int i = 0; i < margin; i++)
            Console.WriteLine();
    }

    public void WriteHorizontalLine(int topMargin, int bottomMargin)
    {
        for (int i = 0; i < topMargin; i++)
            Console.WriteLine();

        Console.WriteLine(new string('-', 79));

        for (int i = 0; i < bottomMargin; i++)
            Console.WriteLine();
    }

    private static void WriteWithColor(ConsoleColor color, string text)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = oldColor;
    }

    private static void WriteLineWithColor(ConsoleColor color, string text)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = oldColor;
    }
}