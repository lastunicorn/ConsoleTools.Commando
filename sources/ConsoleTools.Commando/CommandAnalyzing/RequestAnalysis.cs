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

using DustInTheWind.ConsoleTools.Commando.MetadataModel;
using DustInTheWind.ConsoleTools.Commando.RequestModel;
using ExecutionContext = DustInTheWind.ConsoleTools.Commando.MetadataModel.ExecutionContext;

namespace DustInTheWind.ConsoleTools.Commando.CommandAnalyzing;

internal class RequestAnalysis
{
    private CommandAnalysis matchedCommandAnalysis;

    public CommandMetadata MatchedCommand => matchedCommandAnalysis.Command;

    public RequestMatchType MatchType { get; private set; }

    public List<ParameterMatch> UnmatchedMandatoryParameters { get; } = new();

    public RequestAnalysis(CommandRequest commandRequest, ExecutionContext executionContext)
    {
        if (commandRequest == null) throw new ArgumentNullException(nameof(commandRequest));
        if (executionContext == null) throw new ArgumentNullException(nameof(executionContext));

        matchedCommandAnalysis = null;
        MatchType = RequestMatchType.NoMatch;

        if (commandRequest.IsEmpty)
        {
            CommandMetadata commandMetadata = executionContext.Commands.GetHelpCommand();

            matchedCommandAnalysis = new CommandAnalysis(commandRequest, commandMetadata);
            MatchType = RequestMatchType.Help;
        }
        else
        {
            IEnumerable<CommandMetadata> commandMetadataCollection = string.IsNullOrEmpty(commandRequest.CommandName)
                ? executionContext.Commands.GetAllAnonymous()
                : executionContext.Commands.GetAllByName(commandRequest.CommandName).ToList();

            Analyze(commandRequest, commandMetadataCollection);
        }
    }

    private void Analyze(CommandRequest commandRequest, IEnumerable<CommandMetadata> commandMetadataCollection)
    {
        CommandsAnalysis commandsAnalysis = new(commandRequest, commandMetadataCollection);

        switch (commandsAnalysis.FullMatches.Count)
        {
            case 0:
                switch (commandsAnalysis.PartialMatches.Count)
                {
                    case 0:
                        if (commandsAnalysis.NameMatches.Count > 0)
                        {
                            MatchType = RequestMatchType.OnlyName;
                            UnmatchedMandatoryParameters.AddRange(commandsAnalysis.NameMatches.First().UnmatchedMandatoryParameters);
                        }
                        else
                        {
                            MatchType = RequestMatchType.NoMatch;
                        }
                        break;

                    case 1:
                        matchedCommandAnalysis = commandsAnalysis.PartialMatches.Single();
                        MatchType = RequestMatchType.Partial;
                        break;

                    default:
                        MatchType = RequestMatchType.Multiple;
                        break;
                }
                break;

            case 1:
                matchedCommandAnalysis = commandsAnalysis.FullMatches.Single();
                MatchType = RequestMatchType.Full;
                break;

            default:
                MatchType = RequestMatchType.Multiple;
                break;
        }
    }

    public void SetParameters(object consoleCommand)
    {
        matchedCommandAnalysis?.SetParameters(consoleCommand);
    }
}