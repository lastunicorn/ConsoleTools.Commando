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

using DustInTheWind.ConsoleTools.Commando.Parsing;
using DustInTheWind.ConsoleTools.Commando.RequestModel;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.ConsoleTools.Commando.Tests.Parsing.CommandParserTests.FlagAndOption;

public class CommandParser_HavingTwoCollapsedFlagsWithValueUsingSpace_Tests
{
    private readonly CommandRequest commandRequest;

    public CommandParser_HavingTwoCollapsedFlagsWithValueUsingSpace_Tests()
    {
        // The second flag ("g") is interpreted, for now, as being an ordinary option. The "value1" being its value.
        // Another possible way to interpret these arguments is as being two flags and an operand.

        // To decide between the two possibilities the algorithm needs additional information about "g":
        // - If "g" is a bool => In this case it is a flag and "value1" is an operand.
        // - If "g" is NOT a bool => In this case it is a regular option and "value1" is its value.

        // This type of information about "g" will be available later in the execution flow.
        // So, the decision will be taken later.

        string[] args = { "-fg", "value1" };

        CommandParser commandParser = new();
        commandRequest = commandParser.Parse(args);
    }

    [Fact]
    public void WhenParsed_ThenGenericCommandContainsNullVerb()
    {
        commandRequest.CommandName.Should().BeNull();
    }

    [Fact]
    public void WhenParsed_ThenOptionsListContainsTheTwoFlags()
    {
        CommandArgument[] expected =
        {
            new("f", null),
            new("g", "value1")
        };
        commandRequest.Options.Should().Equal(expected);
    }

    [Fact]
    public void WhenParsed_ThenOperandsListIsEmpty()
    {
        commandRequest.Operands.Should().BeEmpty();
    }
}