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

using System.Reflection;

namespace DustInTheWind.ConsoleTools.Commando.MetadataModel;

/// <summary>
/// The metadata model is used to store information about all the available commands that can be
/// executed. This includes the default commands, provided by the library itself, and, also, the
/// custom commands created by the consumer of the library.
/// </summary>
public class CommandMetadata
{
    private CommandAttribute commandAttribute;
    private CommandOrderAttribute commandOrderAttribute;
    private readonly List<string> descriptionLines;

    public string Name { get; }

    public IReadOnlyList<string> DescriptionLines => descriptionLines;

    public Type Type { get; }

    public CommandKind CommandKind { get; }

    public int Order => commandOrderAttribute?.Order ?? int.MaxValue;

    public bool IsEnabled => commandAttribute?.Enabled ?? true;

    public bool IsHelpCommand { get; private set; }

    public IEnumerable<ParameterMetadata> Parameters
    {
        get
        {
            return Type.GetProperties()
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
        Type = commandType ?? throw new ArgumentNullException(nameof(commandType));

        CommandKind = ComputeCommandKind(commandType);

        if (CommandKind != CommandKind.None)
        {
            RetrieveAttributes();
            Name = ComputeCommandName();
            descriptionLines = ComputeDescription();
        }
    }

    private static CommandKind ComputeCommandKind(Type type)
    {
        if (!type.IsClass || type.IsAbstract)
            return CommandKind.None;

        bool isNonGenericCommand = typeof(IConsoleCommand).IsAssignableFrom(type);

        if (isNonGenericCommand)
            return CommandKind.WithoutResult;

        bool isGenericCommand = type.GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IConsoleCommand<>));

        if (isGenericCommand)
            return CommandKind.WithResult;

        return CommandKind.None;
    }

    private void RetrieveAttributes()
    {
        commandAttribute = Type.GetCustomAttributes(typeof(CommandAttribute), false)
            .Cast<CommandAttribute>()
            .SingleOrDefault();

        IsHelpCommand = commandAttribute is HelpCommandAttribute;

        commandOrderAttribute = Type.GetCustomAttributes(typeof(CommandOrderAttribute), false)
            .Cast<CommandOrderAttribute>()
            .FirstOrDefault();
    }

    private string ComputeCommandName()
    {
        if (commandAttribute is NamedCommandAttribute namedCommandAttribute)
            return namedCommandAttribute.Name;

        return null;

        //if (commandAttribute != null && !string.IsNullOrEmpty(commandAttribute.CommandName))
        //    return commandAttribute.CommandName;

        //if (commandType.Name.EndsWith("Command"))
        //    return commandType.Name[..^"Command".Length].ToLower();

        //return commandType.Name.ToLower();
    }

    private List<string> ComputeDescription()
    {
        List<string> lines = new();

        if (commandAttribute != null && !string.IsNullOrEmpty(commandAttribute.Description))
            lines.Add(commandAttribute.Description);

        return lines;
    }

    public IEnumerable<ParameterMetadata> EnumerateNamedParameters()
    {
        return Parameters
            .Where(x => x.Name != null || x.ShortName != 0);
    }

    public override string ToString()
    {
        return Name ?? "anonymous command " + Order;
    }
}