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

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

public class CommandParameterInfo
{
    public string Name { get; }

    public char ShortName { get; }

    public int? Order { get; }

    [Obsolete("Replaced by the IsMandatory property.")]
    public bool IsOptional { get; }

    public bool IsMandatory { get; }

    public string DisplayName { get; }

    public string Description { get; }

    public Type ParameterType { get; }

    public CommandParameterInfo(ParameterMetadata parameterMetadata)
    {
        Name = parameterMetadata.Name;
        ShortName = parameterMetadata.ShortName;
        DisplayName = parameterMetadata.DisplayName;
        IsOptional = parameterMetadata.IsOptional;
        IsMandatory = parameterMetadata.IsMandatory;
        Order = parameterMetadata.Order;
        Description = parameterMetadata.Description;
        ParameterType = parameterMetadata.ParameterType;
    }
}