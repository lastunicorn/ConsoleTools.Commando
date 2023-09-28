﻿// ConsoleTools.Commando
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

internal class ParameterMatch
{
    private readonly ParameterMetadata parameterMetadata;
    private readonly CommandArgument commandArgument;
    private readonly CommandArgumentType argumentType;

    public bool IsMatch => MatchType is ParameterMatchType.NoButOptional or ParameterMatchType.Yes;

    public ParameterMatchType MatchType { get; set; }

    public ParameterMatch(ParameterMetadata parameterMetadata, CommandRequest commandRequest)
    {
        if (commandRequest == null) throw new ArgumentNullException(nameof(commandRequest));
        this.parameterMetadata = parameterMetadata ?? throw new ArgumentNullException(nameof(parameterMetadata));

        CommandArgument option = commandRequest.GetOptionAndMarkAsUsed(parameterMetadata);

        if (option != null)
        {
            commandArgument = option;
            argumentType = CommandArgumentType.Option;
            MatchType = ParameterMatchType.Yes;
            return;
        }

        CommandArgument operand = commandRequest.GetOperandAndMarkAsUsed2(parameterMetadata);

        if (operand != null)
        {
            commandArgument = operand;
            argumentType = CommandArgumentType.Operand;
            MatchType = ParameterMatchType.Yes;
            return;
        }

        MatchType = parameterMetadata.IsOptional
            ? ParameterMatchType.NoButOptional
            : ParameterMatchType.No;
    }


    public void SetParameter(object consoleCommand)
    {
        switch (MatchType)
        {
            case ParameterMatchType.NoButOptional:
                return;

            case ParameterMatchType.No:
                string parameterName = parameterMetadata.Name ?? parameterMetadata.DisplayName ?? parameterMetadata.Order.ToString();
                throw new ParameterMissingException(parameterName);

            case ParameterMatchType.Yes:
                SetParameterInternal(consoleCommand);
                break;

            default:
                throw new Exception($"Error setting the parameter value. Parameter: {parameterMetadata.Name}.");
        }
    }

    private void SetParameterInternal(object consoleCommand)
    {
        switch (argumentType)
        {
            case CommandArgumentType.Unknown:
                throw new Exception($"Error setting the parameter value. Parameter: {parameterMetadata.Name}.");

            case CommandArgumentType.Option:
                parameterMetadata.SetValue(consoleCommand, commandArgument.Value);
                break;

            case CommandArgumentType.Operand:
                parameterMetadata.SetValue(consoleCommand, commandArgument.Value);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override string ToString()
    {
        return $"Match: {MatchType}; Parameter: {parameterMetadata}";
    }
}