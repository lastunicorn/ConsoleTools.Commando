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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class Arguments : IEnumerable<Argument>
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
        Argument previousArgument = null;

        IEnumerable<Argument> arguments = ExtractArguments(args);

        foreach (Argument argument in arguments)
        {
            if (argument.HasName)
            {
                if (previousArgument != null)
                    yield return previousArgument;

                if (argument.HasValue)
                    yield return argument;
                else
                    previousArgument = argument;
            }
            else
            {
                if (previousArgument != null)
                {
                    yield return new Argument
                    {
                        Name = previousArgument.Name,
                        Value = argument.Value
                    };

                    previousArgument = null;
                }
                else
                {
                    yield return argument;
                }
            }
        }

        if (previousArgument != null)
            yield return previousArgument;
    }

    private static IEnumerable<Argument> ExtractArguments(IEnumerable<string> args)
    {
        foreach (string arg in args)
        {
            ChunkAnalysis chunkAnalysis = new(arg);
            chunkAnalysis.Analyze();

            foreach (Argument argument in chunkAnalysis)
            {
                yield return argument;
            }
        }
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