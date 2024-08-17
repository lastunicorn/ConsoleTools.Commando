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

namespace DustInTheWind.ConsoleTools.Commando.CommandAnalyzing;

internal class ParametersAnalysis
{
    public List<ParameterMatch> UnmatchedMandatory { get; } = new();

    public bool HasUnmatchedMandatory => UnmatchedMandatory.Count > 0;

    public bool HasUnmatchedOptional { get; }

    public ParametersAnalysis(IEnumerable<ParameterMatch> parameterMatches)
    {
        List<ParameterMatch> notMatched = parameterMatches
            .Where(x => !x.IsMatch)
            .ToList();

        foreach (ParameterMatch parameterMatch in notMatched)
        {
            if (parameterMatch.IsParameterMandatory)
                UnmatchedMandatory.Add(parameterMatch);
            else
                HasUnmatchedOptional = true;
        }
    }
}