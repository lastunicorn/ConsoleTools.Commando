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
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.ConsoleTools.Commando;

public partial class EnhancedConsole
{
    public void WriteTitle(string title)
    {
        Console.WriteLine();
        WriteLineWithColor(ConsoleColor.DarkYellow, new string('-', 79));

        if (title != null)
        {
            WriteLineWithColor(ConsoleColor.DarkYellow, $"-> {title}");
            WriteLineWithColor(ConsoleColor.DarkYellow, new string('-', 79));
        }

        Console.WriteLine();
    }

    public void WriteType(string name, object obj)
    {
        DisplayIndentation();
        DisplayValueInternal(name + " (type)", obj?.GetType().FullName);
    }

    private void DisplayValueInternal(string name, object value)
    {
        if (name != null)
        {
            WriteWithColor(DataKeyColor, name + ":");
            Console.Write(" ");
        }

        switch (value)
        {
            case null:
                WriteNullValue();
                Console.WriteLine();
                break;

            case string { Length: 0 }:
                WriteEmptyValue();
                Console.WriteLine();
                break;

            default:
                WriteLineWithColor(DataValueColor, value.ToString());
                break;
        }
    }

    public void WriteValue(string name, byte[] bytes, BinaryDisplayFormat? format = null)
    {
        DisplayIndentation();

        BinaryDisplayFormat computedFormat = format ?? BinaryFormat;

        string encodingName = computedFormat switch
        {
            BinaryDisplayFormat.Base64 => "base64",
            BinaryDisplayFormat.Hexadecimal => "hexa",
            _ => "?"
        };

        WriteWithColor(DataKeyColor, $"{name} ({encodingName}):");
        Console.Write(" ");

        if (bytes == null)
        {
            WriteNullValue();
            Console.WriteLine();
        }
        else
        {
            string text = BinaryToString(bytes, computedFormat);
            WriteLineWithColor(DataValueColor, text);
        }
    }

    private string BinaryToString(byte[] bytes, BinaryDisplayFormat displayBinaryFormat)
    {
        switch (displayBinaryFormat)
        {
            case BinaryDisplayFormat.Hexadecimal:
                {
                    if (BinaryMaxLength is > 0)
                    {
                        int maxLength = BinaryMaxLength.Value;

                        IEnumerable<string> list = bytes
                            .Take(maxLength)
                            .Select(b => $"{b:x2}");

                        string text = string.Join(" ", list);

                        if (bytes.Length > maxLength)
                        {
                            int remainedBytes = bytes.Length - maxLength;
                            text = $"{text}... (+{remainedBytes} bytes)";
                        }

                        return text;
                    }
                    else
                    {
                        IEnumerable<string> list = bytes.Select(b => $"{b:x2}");
                        return string.Join(" ", list);
                    }
                }

            case BinaryDisplayFormat.Base64:
                {
                    string text = Convert.ToBase64String(bytes);

                    if (BinaryMaxLength is > 0)
                    {
                        int maxLength = BinaryMaxLength.Value;

                        if (text.Length > maxLength)
                        {
                            int remainedLength = text.Length - maxLength;
                            text = text[..maxLength] + "... (+" + remainedLength + " chars)";
                        }
                    }

                    return text;
                }

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void WriteValueBelowName(string name, object value)
    {
        DisplayIndentation();
        WriteLineWithColor(DataKeyColor, name + ":");

        switch (value)
        {
            case null:
                WriteNullValue();
                Console.WriteLine();
                break;

            case string { Length: 0 }:
                WriteEmptyValue();
                Console.WriteLine();
                break;

            default:
                WriteLineWithColor(DataValueColor, value.ToString());
                break;
        }
    }

    private void WriteNullValue()
    {
        WriteWithColor(NullOrEmptyColor, "<null>");
    }

    private void WriteEmptyValue()
    {
        WriteWithColor(NullOrEmptyColor, "<empty>");
    }
}