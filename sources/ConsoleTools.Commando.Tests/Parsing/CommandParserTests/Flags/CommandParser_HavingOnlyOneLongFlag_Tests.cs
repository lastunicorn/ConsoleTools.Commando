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

using DustInTheWind.ConsoleTools.Commando.Parsing;
using DustInTheWind.ConsoleTools.Commando.RequestModel;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.ConsoleTools.Commando.Tests.Parsing.CommandParserTests.Flags;

public class CommandParser_HavingOnlyOneLongFlag_Tests
{
    private readonly CommandRequest commandRequest;

    public CommandParser_HavingOnlyOneLongFlag_Tests()
    {
        string[] args = { "--flag1" };

        CommandParser commandParser = new();
        commandRequest = commandParser.Parse(args);
    }

    [Fact]
    public void WhenParsed_ThenGenericCommandContainsNullVerb()
    {
        commandRequest.CommandName.Should().BeNull();
    }

    [Fact]
    public void WhenParsed_ThenOptionsListContainsTheOption()
    {
        CommandArgument[] expected =
        {
            new("flag1", null)
        };
        commandRequest.Options.Should().Equal(expected);
    }

    [Fact]
    public void WhenParsed_ThenOperandsListIsEmpty()
    {
        commandRequest.Operands.Should().BeEmpty();
    }
}