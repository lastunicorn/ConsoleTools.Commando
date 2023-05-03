// Velo City
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

using DustInTheWind.ConsoleTools.Commando.CommandRequestModel;
using DustInTheWind.ConsoleTools.Commando.Parsing;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.ConsoleTools.Commando.Tests.Parsing;

public class CommandParser_Verb_NoOptions_NoOperands_Tests
{
    [Theory]
    [InlineData("read")]
    [InlineData("write")]
    public void HavingOnlyTheVerb_WhenParsed_ThenGenericCommandContainsTheVerb(string verb)
    {
        string[] args = { verb };

        CommandParser commandParser = new();
        CommandRequest commandRequest = commandParser.Parse(args);

        commandRequest.Verb.Should().Be(verb);
    }

    [Theory]
    [InlineData("read")]
    [InlineData("write")]
    public void HavingOnlyTheVerb_WhenParsed_ThenGenericCommandContainsEmptyOptionsList(string verb)
    {
        string[] args = { verb };

        CommandParser commandParser = new();
        CommandRequest commandRequest = commandParser.Parse(args);

        commandRequest.Verb.Should().Be(verb);

        commandRequest.Options.Should().BeEmpty();
    }

    [Theory]
    [InlineData("read")]
    [InlineData("write")]
    public void HavingOnlyTheVerb_WhenParsed_ThenGenericCommandContainsEmptyOperandsList(string verb)
    {
        string[] args = { verb };

        CommandParser commandParser = new();
        CommandRequest commandRequest = commandParser.Parse(args);

        commandRequest.Verb.Should().Be(verb);

        commandRequest.Operands.Should().BeEmpty();
    }
}