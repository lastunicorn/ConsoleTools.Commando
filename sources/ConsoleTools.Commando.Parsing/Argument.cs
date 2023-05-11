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

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class Argument
{

    private readonly string value;

    //private int nameStartIndex;
    //private int nameEndIndex;
    //private int valueStartIndex;
    //private int valueEndIndex;

    //int index;

    public string Name { get; set; }

    public string Value { get; set; }

    public bool IsNamedArgument => Name != null;

    public bool IsAnonymousArgument => Name == null;

    public bool HasName => Name != null;

    public bool HasValue => Value != null;

    public Argument()
    {
    }

    public Argument(string value)
    {
        this.value = value;

        if (value == null)
            return;

        Parse();
    }

    private void Parse()
    {
        string trimmedValue = value.Trim();

        if (trimmedValue.StartsWith("--"))
        {
            trimmedValue = trimmedValue[2..];
            ExtractNameAndValue(trimmedValue);
        }
        else if (trimmedValue.StartsWith('-'))
        {
            trimmedValue = trimmedValue[1..];
            ExtractNameAndValue(trimmedValue);
        }
        else if (trimmedValue.StartsWith('/'))
        {
            trimmedValue = trimmedValue[1..];
            ExtractNameAndValue(trimmedValue);
        }
        else
        {
            Value = trimmedValue;
        }
    }

    private void ExtractNameAndValue(string value)
    {
        int separatorIndex = value.IndexOf(':');

        if (separatorIndex < 0)
            separatorIndex = value.IndexOf('=');

        if (separatorIndex >= 0)
        {
            Name = value[..separatorIndex];
            Value = value[(separatorIndex + 1)..];
        }
        else
        {
            Name = value;
        }
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

    public override string ToString()
    {
        string argumentType = string.Empty;

        if (IsNamedArgument)
            argumentType += "Named";

        if (IsAnonymousArgument)
            argumentType += "Anonymous";

        if (Name != null && Value != null)
            return $"{Name} = {Value} []";

        if (Name != null)
            return $"{Name} [{argumentType}]";

        if (Value != null)
            return $"{Value} [{argumentType}]";

        return $"null [{argumentType}]";
    }
}