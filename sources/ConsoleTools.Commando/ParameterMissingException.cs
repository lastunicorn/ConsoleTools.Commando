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

namespace DustInTheWind.ConsoleTools.Commando;

internal class ParameterMissingException : Exception
{
    public ParameterMissingException(ICollection<string> parameterNames)
        : base(BuildMessage(parameterNames))
    {
    }

    private static string BuildMessage(ICollection<string> parameterNames)
    {
        switch (parameterNames.Count)
        {
            case 0:
                return Resources.ErrorMessage_ParameterMissing_0;

            case 1:
            {
                string parameterName = parameterNames.First();
                return string.Format(Resources.ErrorMessage_ParameterMissing_1, parameterName);
            }

            case > 1:
            {
                string parameterNamesAsString = string.Join(", ", parameterNames);
                return string.Format(Resources.ErrorMessage_ParameterMissing_N, parameterNamesAsString);
            }

            default:
                return Resources.ErrorMessage_ParameterMissing_0;
        }
    }
}