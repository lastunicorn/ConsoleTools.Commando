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

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class Argument
{
    public string Name { get; init; }

    public string Value { get; init; }

    public bool IsNamedArgument => Name != null;

    public bool IsAnonymousArgument => Name == null;

    public bool IsForcedToBeAnonymous { get; init; }

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