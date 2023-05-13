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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class ChunkAnalysis : IEnumerable<Argument>
{
    private readonly string rawText;

    private readonly List<Argument> arguments = new();

    //private int nameStartIndex;
    //private int nameEndIndex;
    //private int valueStartIndex;
    //private int valueEndIndex;

    //int index;

    public ChunkAnalysis(string text)
    {
        rawText = text;
    }

    public void Analyze()
    {
        arguments.Clear();

        if (rawText == null)
            return;

        string trimmedValue = rawText.Trim();

        if (trimmedValue.StartsWith("--"))
        {
            trimmedValue = trimmedValue[2..];
            (string name, string value) = ExtractNameAndValue(trimmedValue);

            Argument argument = new()
            {
                Name = name,
                Value = value
            };
            arguments.Add(argument);
        }
        else if (trimmedValue.StartsWith('/'))
        {
            trimmedValue = trimmedValue[1..];
            (string name, string value) = ExtractNameAndValue(trimmedValue);

            Argument argument = new()
            {
                Name = name,
                Value = value
            };
            arguments.Add(argument);
        }
        else if (trimmedValue.StartsWith('-'))
        {
            trimmedValue = trimmedValue[1..];
            (string name, string value) = ExtractNameAndValue(trimmedValue);

            name
                .Distinct()
                .ForEach((c, index, isLast) =>
                {
                    Argument argument;

                    if (isLast)
                    {
                        argument = new Argument
                        {
                            Name = c.ToString(),
                            Value = value
                        };
                    }
                    else
                    {
                        argument = new Argument
                        {
                            Name = c.ToString()
                        };
                    }

                    arguments.Add(argument);
                });
        }
        else
        {
            arguments.Add(new Argument
            {
                Value = trimmedValue
            });
        }
    }

    private static (string, string) ExtractNameAndValue(string text)
    {
        int separatorIndex = text.IndexOf(':');

        if (separatorIndex < 0)
            separatorIndex = text.IndexOf('=');

        if (separatorIndex < 0)
            return (text, null);

        string name = text[..separatorIndex];
        string value = text[(separatorIndex + 1)..];

        return (name, value);
    }

    //public void Analyze()
    //{
    //    nameStartIndex = -1;
    //    nameEndIndex = -1;
    //    valueStartIndex = -1;
    //    valueEndIndex = -1;

    //    if (value == null)
    //        return;

    //    index = 0;

    //    SkipWhiteSpaces();

    //    if (index == value.Length)
    //        return;
    //}

    //private void SkipWhiteSpaces()
    //{
    //    while (true)
    //    {
    //        if (index == value.Length)
    //            return;

    //        if (value[index] is ' ' or '\t')
    //            index++;
    //    }
    //}
    public IEnumerator<Argument> GetEnumerator()
    {
        return arguments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}