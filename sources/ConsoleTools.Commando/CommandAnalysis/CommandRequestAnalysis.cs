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

using DustInTheWind.ConsoleTools.Commando.MetadataModel;
using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando.CommandAnalysis;

internal class CommandRequestAnalysis
{
    private readonly CommandMetadataCollection commandMetadataCollection;
    private readonly List<CommandMetadata> fullMatches = new();
    private readonly List<CommandMetadata> partialMatches = new();

    public CommandMetadata Match { get; private set; }

    public CommandMatchType MatchType { get; private set; }

    public CommandRequestAnalysis(CommandMetadataCollection commandMetadataCollection)
    {
        this.commandMetadataCollection = commandMetadataCollection ?? throw new ArgumentNullException(nameof(commandMetadataCollection));
    }

    public void Analyze(CommandRequest commandRequest)
    {
        fullMatches.Clear();
        partialMatches.Clear();
        Match = null;
        MatchType = CommandMatchType.None;

        IEnumerable<CommandMetadata> commandMetadataList = string.IsNullOrEmpty(commandRequest.CommandName)
            ? commandMetadataCollection.GetAnonymous()
            : commandMetadataCollection.GetAllByName(commandRequest.CommandName);

        foreach (CommandMetadata commandMetadata in commandMetadataList)
            Analyze(commandRequest, commandMetadata);

        switch (fullMatches.Count)
        {
            case 0:
                switch (partialMatches.Count)
                {
                    case 0:
                        MatchType = CommandMatchType.None;
                        break;

                    case 1:
                        Match = partialMatches.Single();
                        MatchType = CommandMatchType.Partial;
                        break;

                    default:
                        MatchType = CommandMatchType.Multiple;
                        break;
                }
                break;

            case 1:
                Match = fullMatches.Single();
                MatchType = CommandMatchType.Full;
                break;

            default:
                MatchType = CommandMatchType.Multiple;
                break;
        }
    }

    private void Analyze(CommandRequest commandRequest, CommandMetadata commandMetadata)
    {
        commandRequest.Reset();

        int missingCount = 0;
        int optionalCount = 0;

        foreach (ParameterMetadata parameterMetadata in commandMetadata.Parameters)
        {
            bool isMatch = IsMatch(parameterMetadata, commandRequest);

            if (isMatch)
                continue;

            if (parameterMetadata.IsOptional)
                optionalCount++;
            else
                missingCount++;
        }

        if (missingCount > 0)
            return;

        if (optionalCount > 0 || commandRequest.HasUnusedArguments)
            partialMatches.Add(commandMetadata);
        else
            fullMatches.Add(commandMetadata);
    }

    private static bool IsMatch(ParameterMetadata parameterMetadata, CommandRequest commandRequest)
    {
        CommandArgument commandArgument = commandRequest.GetOptionAndMarkAsUsed(parameterMetadata);

        if (commandArgument != null)
            return true;

        string operand = commandRequest.GetOperandAndMarkAsUsed(parameterMetadata);

        if (operand != null)
            return true;

        return parameterMetadata.IsOptional;
    }
}