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

namespace DustInTheWind.ConsoleTools.Commando.CommandAnalyzing;

internal class CommandsAnalysis
{
    public List<CommandAnalysis> FullMatches { get; } = new();

    public List<CommandAnalysis> PartialMatches { get; } = new();

    public List<CommandAnalysis> NameMatches { get; } = new();

    public CommandsAnalysis(CommandRequest commandRequest, IEnumerable<CommandMetadata> commandMetadataCollection)
    {
        foreach (CommandMetadata commandMetadata in commandMetadataCollection)
            Analyze(commandRequest, commandMetadata);
    }

    private void Analyze(CommandRequest commandRequest, CommandMetadata commandMetadata)
    {
        CommandAnalysis commandAnalysis = new(commandRequest, commandMetadata);

        switch (commandAnalysis.MatchType)
        {
            case CommandMatchType.NoMatch:
                break;

            case CommandMatchType.Partial:
                PartialMatches.Add(commandAnalysis);
                break;

            case CommandMatchType.Full:
                FullMatches.Add(commandAnalysis);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        if (commandAnalysis.Command.Name == commandRequest.CommandName)
            NameMatches.Add(commandAnalysis);
    }
}