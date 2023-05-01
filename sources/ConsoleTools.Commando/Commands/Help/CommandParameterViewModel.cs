// ConsoleTools.Commando
// Copyright (C) 2022 Dust in the Wind
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

using System.Text;
using DustInTheWind.ConsoleTools.Commando.CommandMetadataModel;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help
{
    internal class CommandParameterViewModel
    {
        private readonly ParameterMetadata parameterMetadata;

        public bool DisplayAsNamedParameter { get; set; }

        public CommandParameterViewModel(ParameterMetadata parameterMetadata)
        {
            this.parameterMetadata = parameterMetadata;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            bool isNamedParameter = parameterMetadata.Order == null || DisplayAsNamedParameter;
            bool isOptionalParameter = parameterMetadata.IsOptional;
            bool hasMultipleNames = isNamedParameter && parameterMetadata.Name != null && parameterMetadata.ShortName != 0;

            if (isOptionalParameter)
                sb.Append('[');
            else if (hasMultipleNames)
                sb.Append('(');

            if (isNamedParameter)
            {
                string computedParameterName = SerializeNamedParameter();
                sb.Append(computedParameterName);
            }
            else
            {
                string computedParameterName = SerializeUnnamedParameter();
                sb.Append(computedParameterName);
            }

            if (isOptionalParameter)
                sb.Append(']');
            else if (hasMultipleNames)
                sb.Append(')');

            return sb.ToString();
        }

        private string SerializeUnnamedParameter()
        {
            StringBuilder sb = new();

            sb.Append('<');

            if (parameterMetadata.DisplayName != null)
                sb.Append(parameterMetadata.DisplayName.Replace(' ', '-'));
            else if (parameterMetadata.Name != null)
                sb.Append(parameterMetadata.Name);
            else if (parameterMetadata.ShortName != 0)
                sb.Append(parameterMetadata.ShortName);
            else
                sb.Append("param" + parameterMetadata.Order);

            sb.Append('>');

            return sb.ToString();
        }

        private string SerializeNamedParameter()
        {
            StringBuilder sb = new();

            if (parameterMetadata.Name != null)
            {
                sb.Append($"-{parameterMetadata.Name}");

                string valueDescription = SerializeValueDescription();
                sb.Append(valueDescription);
            }

            if (parameterMetadata.ShortName != 0)
            {
                if (parameterMetadata.Name != null)
                    sb.Append(" | ");

                sb.Append($"-{parameterMetadata.ShortName}");

                string valueDescription = SerializeValueDescription();
                sb.Append(valueDescription);
            }

            return sb.ToString();
        }

        private string SerializeValueDescription()
        {
            if (parameterMetadata.ParameterType.IsText())
                return (" <text>");

            if (parameterMetadata.ParameterType.IsNumber())
                return (" <number>");

            if (parameterMetadata.ParameterType.IsListOfNumbers())
                return (" <list-of-numbers>");

            if (parameterMetadata.ParameterType.IsListOfTexts())
                return (" <list-of-texts>");

            if (parameterMetadata.ParameterType.IsBoolean())
                return string.Empty;

            return (" <value>");
        }
    }
}