﻿// Velo City
// Copyright (C) 2022 Dust in the Wind
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

using DustInTheWind.ConsoleTools.Commando.GenericCommandModel;
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
        GenericCommand genericCommand = commandParser.Parse(args);

        genericCommand.Verb.Should().Be(verb);
    }

    [Theory]
    [InlineData("read")]
    [InlineData("write")]
    public void HavingOnlyTheVerb_WhenParsed_ThenGenericCommandContainsEmptyOptionsList(string verb)
    {
        string[] args = { verb };

        CommandParser commandParser = new();
        GenericCommand genericCommand = commandParser.Parse(args);

        genericCommand.Verb.Should().Be(verb);

        genericCommand.Options.Should().BeEmpty();
    }

    [Theory]
    [InlineData("read")]
    [InlineData("write")]
    public void HavingOnlyTheVerb_WhenParsed_ThenGenericCommandContainsEmptyOperandsList(string verb)
    {
        string[] args = { verb };

        CommandParser commandParser = new();
        GenericCommand genericCommand = commandParser.Parse(args);

        genericCommand.Verb.Should().Be(verb);

        genericCommand.Operands.Should().BeEmpty();
    }
}