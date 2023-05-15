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

using DustInTheWind.ConsoleTools.Commando.CommandRequestModel;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class TextCommandAnalysis
{
    private readonly Arguments arguments;

    public TextCommandAnalysis(string[] args)
    {
        if (args == null) throw new ArgumentNullException(nameof(args));

        arguments = new Arguments(args);
    }

    public CommandRequest Analyze()
    {
        CommandRequest commandRequest = new()
        {
            UnderlyingArgs = arguments.UnderlyingArgs
        };

        bool isFirst = true;

        foreach (Argument argument in arguments)
        {
            if (isFirst)
            {
                isFirst = false;

                if (argument.IsAnonymousArgument && !argument.IsForcedToBeAnonymous)
                {
                    commandRequest.Verb = argument.Value;
                    continue;
                }
            }

            CommandArgument commandArgument = new(argument.Name, argument.Value);
            commandRequest.AddParameter(commandArgument);
        }

        return commandRequest;
    }
}