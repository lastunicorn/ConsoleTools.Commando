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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ConsoleTools.Commando.CommandMetadataModel;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help
{
    public class CommandUsageViewModel
    {
        private readonly CommandMetadata commandMetadata;
        private readonly string applicationName;

        public CommandUsageViewModel(CommandMetadata commandMetadata, string applicationName)
        {
            this.commandMetadata = commandMetadata;
            this.applicationName = applicationName;
        }

        public override string ToString()
        {
            if (commandMetadata == null)
                return string.Empty;

            StringBuilder sb = new($"{applicationName} {commandMetadata.Name}");

            IEnumerable<CommandParameterViewModel> ordinalParameters = commandMetadata.Parameters
                .Where(x => x.Order != null)
                .OrderBy(x => x.Order)
                .Select(x => new CommandParameterViewModel(x));

            foreach (CommandParameterViewModel parameterInfo in ordinalParameters)
            {
                sb.Append(' ');
                sb.Append(parameterInfo);
            }

            IEnumerable<CommandParameterViewModel> namedParameters = commandMetadata.Parameters
                .Where(x => x.Name != null || x.ShortName != 0)
                .Select(x => new CommandParameterViewModel(x)
                {
                    DisplayAsNamedParameter = true
                });

            foreach (CommandParameterViewModel parameterInfo in namedParameters)
            {
                sb.Append(' ');
                sb.Append(parameterInfo);
            }

            return sb.ToString();
        }
    }
}