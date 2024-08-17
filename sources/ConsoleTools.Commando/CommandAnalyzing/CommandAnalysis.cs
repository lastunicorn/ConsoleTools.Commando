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

internal class CommandAnalysis
{
    private readonly List<ParameterMatch> parameterMatches;

    public CommandMatchType MatchType { get; }

    public CommandMetadata Command { get; set; }

    public List<ParameterMatch> UnmatchedMandatoryParameters { get; } = new();

    public CommandAnalysis(CommandRequest commandRequest, CommandMetadata commandMetadata)
    {
        if (commandRequest == null) throw new ArgumentNullException(nameof(commandRequest));
        Command = commandMetadata ?? throw new ArgumentNullException(nameof(commandMetadata));

        commandRequest.Reset();

        parameterMatches = commandMetadata.Parameters
            .Select(x => new ParameterMatch(x, commandRequest))
            .ToList();

        ParametersAnalysis parametersAnalysis = new(parameterMatches);

        if (parametersAnalysis.HasUnmatchedMandatory)
        {
            UnmatchedMandatoryParameters.AddRange(parametersAnalysis.UnmatchedMandatory);
            MatchType = CommandMatchType.NoMatch;
        }
        else if (parametersAnalysis.HasUnmatchedOptional)
        {
            MatchType = CommandMatchType.Partial;
        }
        else
        {
            MatchType = CommandMatchType.Full;
        }
    }

    public void SetParameters(object consoleCommand)
    {
        IEnumerable<ParameterMatch> validParameters = parameterMatches.Where(x => x.IsMatch);

        foreach (ParameterMatch parameterMatch in validParameters)
            parameterMatch.SetParameter(consoleCommand);
    }
}