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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DustInTheWind.ConsoleTools.Commando.CommandMetadataModel;

/// <summary>
/// The metadata model is used to store information about all the available commands that can be
/// executed. This includes the default commands, provided by the library itself, and, also, the
/// custom commands created by the consumer of the library.
/// </summary>
public class CommandMetadata
{
    private readonly Type commandType;
    private CommandAttribute commandAttribute;
    private readonly List<string> descriptionLines;

    public string Name { get; }

    public IReadOnlyList<string> DescriptionLines => descriptionLines;

    public Type Type { get; }

    public int Order => commandAttribute?.Order ?? int.MaxValue;

    public bool IsEnabled => commandAttribute?.Enabled ?? true;

    public bool IsHelpCommand { get; private set; }

    public IEnumerable<ParameterMetadata> Parameters
    {
        get
        {
            return commandType.GetProperties()
                .Select(x =>
                {
                    CommandParameterAttribute customAttribute = x.GetCustomAttributes<CommandParameterAttribute>()
                        .SingleOrDefault();

                    return customAttribute == null
                        ? null
                        : new ParameterMetadata(x, customAttribute);
                })
                .Where(x => x != null)
                .ToArray();
        }
    }

    public CommandMetadata(Type commandType)
    {
        this.commandType = commandType ?? throw new ArgumentNullException(nameof(commandType));

        RetrieveAttributes();

        Name = ComputeCommandName();
        descriptionLines = ComputeDescription();
        Type = commandType;
    }

    private void RetrieveAttributes()
    {
        commandAttribute = commandType.GetCustomAttributes(typeof(CommandAttribute), false)
            .Cast<CommandAttribute>()
            .SingleOrDefault();

        IsHelpCommand = commandAttribute is HelpCommandAttribute;
    }

    private string ComputeCommandName()
    {
        if (commandAttribute != null && !string.IsNullOrEmpty(commandAttribute.CommandName))
            return commandAttribute.CommandName;

        if (commandType.Name.EndsWith("Command"))
            return commandType.Name[..^"Command".Length].ToLower();

        return commandType.Name.ToLower();
    }

    private List<string> ComputeDescription()
    {
        List<string> lines = new();

        if (commandAttribute != null && !string.IsNullOrEmpty(commandAttribute.ShortDescription))
            lines.Add(commandAttribute.ShortDescription);

        return lines;
    }

    public IEnumerable<ParameterMetadata> EnumerateNamedParameters()
    {
        return Parameters
            .Where(x => x.Name != null || x.ShortName != 0);
    }

    public override string ToString()
    {
        return Name ?? "param" + Order;
    }
}