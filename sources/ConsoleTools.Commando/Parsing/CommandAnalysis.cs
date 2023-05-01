// ConsoleTools.Commando
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

using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ConsoleTools.Commando.GenericCommandModel;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class CommandAnalysis
{
    private readonly Arguments arguments;

    public CommandAnalysis(string[] args)
    {
        if (args == null) throw new ArgumentNullException(nameof(args));

        arguments = new Arguments(args);
    }

    public GenericCommand Analyze()
    {
        Argument verbArgument = GetVerb();

        GenericCommand genericCommand = new()
        {
            UnderlyingArgs = arguments.UnderlyingArgs,
            Verb = verbArgument?.Value
        };

        IEnumerable<GenericCommandOption> options = GetOptions();
        genericCommand.Options.AddRange(options);

        IEnumerable<string> operands = GetOperands(verbArgument);
        genericCommand.Operands.AddRange(operands);

        return genericCommand;
    }

    public Argument GetVerb()
    {
        if (arguments.Count == 0)
            return null;

        Argument verbArgument = arguments[0];

        if (verbArgument.Type != ArgumentType.Ordinal)
            return null;

        return verbArgument;
    }

    private IEnumerable<GenericCommandOption> GetOptions()
    {
        return arguments
            .Where(x => x.Type == ArgumentType.Named)
            .Select(x => new GenericCommandOption(x.Name, x.Value));
    }

    private IEnumerable<string> GetOperands(Argument verbArgument)
    {
        IEnumerable<Argument> query = arguments
            .Where(x => x.Type == ArgumentType.Ordinal);

        bool skipFirsArgument = !string.IsNullOrEmpty(verbArgument?.Value);

        if (skipFirsArgument)
            query = query.Skip(1);

        return query.Select(x => x.Value);
    }
}