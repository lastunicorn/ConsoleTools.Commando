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
    private readonly ExecutionMetadata executionMetadata;
    private readonly List<CommandMetadata> fullMatches = new();
    private readonly List<CommandMetadata> partialMatches = new();
    private List<ParameterMatch> parameterMatches;

    public CommandMetadata MatchedCommand { get; private set; }

    public CommandMatchType MatchType { get; private set; }

    public CommandRequestAnalysis(ExecutionMetadata executionMetadata)
    {
        this.executionMetadata = executionMetadata ?? throw new ArgumentNullException(nameof(executionMetadata));
    }

    public void Analyze(CommandRequest commandRequest)
    {
        fullMatches.Clear();
        partialMatches.Clear();
        MatchedCommand = null;
        MatchType = CommandMatchType.None;
        parameterMatches = null;

        if (commandRequest.IsEmpty)
        {
            MatchedCommand = executionMetadata.Commands.GetHelpCommand();
            MatchType = CommandMatchType.Help;
        }
        else
        {
            IEnumerable<CommandMetadata> commandMetadataList = string.IsNullOrEmpty(commandRequest.CommandName)
                ? executionMetadata.Commands.GetAllAnonymous()
                : executionMetadata.Commands.GetAllByName(commandRequest.CommandName);

            foreach (CommandMetadata commandMetadata in commandMetadataList)
                Analyze(commandRequest, commandMetadata);

            ComputeAnalysisResult();
        }
    }

    private void Analyze(CommandRequest commandRequest, CommandMetadata commandMetadata)
    {
        commandRequest.Reset();

        int optionalCount = 0;

        parameterMatches = commandMetadata.Parameters
            .Select(x => new ParameterMatch(x, commandRequest))
            .ToList();

        foreach (ParameterMatch parameterMatch in parameterMatches)
        {
            if (parameterMatch.IsMatch)
                continue;

            if (parameterMatch.MatchType == ParameterMatchType.NoButOptional)
                optionalCount++;
        }

        if (optionalCount > 0 || commandRequest.HasUnusedArguments)
            partialMatches.Add(commandMetadata);
        else
            fullMatches.Add(commandMetadata);
    }

    private void ComputeAnalysisResult()
    {
        switch (fullMatches.Count)
        {
            case 0:
                switch (partialMatches.Count)
                {
                    case 0:
                        MatchType = CommandMatchType.None;
                        break;

                    case 1:
                        MatchedCommand = partialMatches.Single();
                        MatchType = CommandMatchType.Partial;
                        break;

                    default:
                        MatchType = CommandMatchType.Multiple;
                        break;
                }
                break;

            case 1:
                MatchedCommand = fullMatches.Single();
                MatchType = CommandMatchType.Full;
                break;

            default:
                MatchType = CommandMatchType.Multiple;
                break;
        }
    }

    public void SetParameters(object consoleCommand)
    {
        if (parameterMatches == null)
            return;

        foreach (ParameterMatch parameterMatch in parameterMatches)
            parameterMatch.SetParameter(consoleCommand);
    }
}