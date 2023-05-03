using System;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.ConsoleTools.Commando.Demo;

public abstract partial class ViewBase<TCommand>
    where TCommand : ICommand
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

        if (value == null)
            Console.WriteLine("<null>");
        else
            WriteLineWithColor(DataValueColor, value.ToString());
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
            Console.WriteLine("<null>");
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

        if (value == null)
            Console.WriteLine("<null>");
        else
            WriteLineWithColor(DataValueColor, value.ToString());
    }
}