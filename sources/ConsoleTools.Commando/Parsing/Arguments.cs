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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

public class Arguments : IEnumerable<Argument>
{
    public string[] UnderlyingArgs { get; }

    private readonly List<Argument> arguments = new();

    public int Count => arguments.Count;

    public Argument this[int index]
    {
        get
        {
            bool isValidIndex = index >= 0 && index < arguments.Count;

            return isValidIndex
                ? arguments[index]
                : null;
        }
    }

    public Argument this[string name] => arguments.FirstOrDefault(x => x.Name == name);

    public Arguments(string[] args)
    {
        UnderlyingArgs = args ?? throw new ArgumentNullException(nameof(args));

        IEnumerable<Argument> newArguments = Parse(args);
        arguments.AddRange(newArguments);
    }

    private static IEnumerable<Argument> Parse(IEnumerable<string> args)
    {
        Argument argument = null;

        IEnumerable<ChunkAnalysis> chunks = args.Select(x => new ChunkAnalysis(x));

        foreach (ChunkAnalysis chunk in chunks)
        {
            if (chunk.HasName)
            {
                if (argument != null)
                    yield return argument;

                if (chunk.HasValue)
                {
                    argument = new Argument
                    {
                        Name = chunk.Name,
                        Value = chunk.Value
                    };

                    yield return argument;
                    argument = null;
                }
                else
                {
                    argument = new Argument
                    {
                        Name = chunk.Name
                    };
                }
            }
            else
            {
                argument ??= new Argument();

                argument.Value = chunk.Value;

                yield return argument;
                argument = null;
            }
        }

        if (argument != null)
            yield return argument;
    }

    public IEnumerator<Argument> GetEnumerator()
    {
        return arguments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}