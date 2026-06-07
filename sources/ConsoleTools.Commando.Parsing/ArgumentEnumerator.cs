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

using System.Collections;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class ArgumentEnumerator : IEnumerator<Argument>
{
    private readonly IEnumerable<string> args;

    private IEnumerator<string> argsEnumerator;

    private readonly Queue<Argument> extractedArguments = new();

    private bool forceToBeOperand;

    private Argument previousArgument;

    public ArgumentEnumerator(IEnumerable<string> args)
    {
        this.args = args ?? throw new ArgumentNullException(nameof(args));
        argsEnumerator = this.args.GetEnumerator();
    }

    public bool MoveNext()
    {
        while (true)
        {
            if (!TryExtractNextArgument(out Argument argument))
            {
                if (previousArgument != null)
                {
                    Current = previousArgument;
                    previousArgument = null;
                    return true;
                }

                Current = null;
                return false;
            }

            if (argument.IsNamedArgument)
            {
                if (previousArgument != null)
                {
                    Current = previousArgument;
                    previousArgument = argument.Value == null ? argument : null;

                    if (argument.Value != null)
                        extractedArguments.Enqueue(argument);

                    return true;
                }

                if (argument.Value != null)
                {
                    Current = argument;
                    return true;
                }

                previousArgument = argument;
            }
            else
            {
                if (previousArgument != null)
                {
                    Current = new Argument
                    {
                        Name = previousArgument.Name,
                        Value = argument.Value
                    };
                    previousArgument = null;
                    return true;
                }

                Current = argument;
                return true;
            }
        }
    }

    public void Reset()
    {
        argsEnumerator.Dispose();
        argsEnumerator = args.GetEnumerator();
        extractedArguments.Clear();
        forceToBeOperand = false;
        previousArgument = null;
        Current = null;
    }

    public Argument Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        argsEnumerator.Dispose();
    }

    private bool TryExtractNextArgument(out Argument argument)
    {
        if (extractedArguments.Count > 0)
        {
            argument = extractedArguments.Dequeue();
            return true;
        }

        while (argsEnumerator.MoveNext())
        {
            string arg = argsEnumerator.Current;

            if (arg == "--")
            {
                forceToBeOperand = true;
                continue;
            }

            if (forceToBeOperand)
            {
                argument = new Argument
                {
                    Value = arg,
                    IsForcedToBeAnonymous = true
                };
                return true;
            }

            ChunkAnalysis chunkAnalysis = new(arg);
            chunkAnalysis.Analyze();

            foreach (Argument newArgument in chunkAnalysis)
                extractedArguments.Enqueue(newArgument);

            if (extractedArguments.Count > 0)
            {
                argument = extractedArguments.Dequeue();
                return true;
            }
        }

        argument = null;
        return false;
    }
}