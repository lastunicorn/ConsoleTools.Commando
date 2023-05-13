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

public class CommandMetadataCollection
{
    private readonly List<CommandMetadata> commandsMetadata = new();
    private readonly List<Type> viewTypes = new();
    private bool isFrozen;

    public void LoadFromCurrentAppDomain()
    {
        if (isFrozen)
            throw new InvalidOperationException();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        LoadFrom(assemblies);
    }

    public void LoadFrom(params Assembly[] assemblies)
    {
        if (isFrozen)
            throw new InvalidOperationException();

        IEnumerable<Type> allTypes = assemblies
            .SelectMany(x => x.GetTypes());

        foreach (Type type in allTypes)
        {
            if (IsCommandType(type))
            {
                CommandMetadata commandMetadata = new(type);
                commandsMetadata.Add(commandMetadata);
            }

            if (IsViewType(type))
                viewTypes.Add(type);
        }
    }

    public void Clear()
    {
        if (isFrozen)
            throw new InvalidOperationException();

        commandsMetadata.Clear();
        viewTypes.Clear();
    }

    private static bool IsCommandType(Type type)
    {
        return !type.IsAbstract &&
               type != typeof(ICommand) &&
               typeof(ICommand).IsAssignableFrom(type);
    }

    private static bool IsViewType(Type type)
    {
        if (type.IsAbstract)
            return false;

        Type[] interfaceTypes = type.GetInterfaces();

        foreach (Type interfaceType in interfaceTypes)
        {
            bool isGenericType = interfaceType.IsGenericType;

            if (!isGenericType)
                continue;

            Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();

            return genericTypeDefinition == typeof(IView<>);
        }

        return false;
    }

    public IEnumerable<Type> GetCommandTypes()
    {
        return commandsMetadata
            .Select(x => x.Type);
    }

    public IEnumerable<Type> GetViewTypes()
    {
        return viewTypes.AsReadOnly();
    }

    public IEnumerable<Type> GetViewTypesForCommand(Type commandType)
    {
        return viewTypes
            .Where(x =>
            {
                IEnumerable<Type> interfaceTypes = x.GetInterfaces();

                foreach (Type interfaceType in interfaceTypes)
                {
                    Type[] genericArgumentTypes = interfaceType.GetGenericArguments();

                    if (genericArgumentTypes.Length != 1)
                        continue;

                    if (genericArgumentTypes[0] != commandType)
                        continue;

                    return true;
                }

                return false;
            });
    }

    public CommandMetadata GetByName(string commandName)
    {
        return commandsMetadata
            .Where(x => x.IsEnabled && x.Name == commandName)
            .MinBy(x => x.Order);
    }

    public IEnumerable<CommandMetadata> GetEnabledNamed()
    {
        return commandsMetadata
            .Where(x => x.IsEnabled && x.Name != null)
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Name);
    }

    public IEnumerable<CommandMetadata> GetEnabledDefault()
    {
        return commandsMetadata
            .Where(x => x.IsEnabled && x.Name == null)
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Name);
    }

    public CommandMetadata GetHelpCommand()
    {
        CommandMetadata commandMetadata = commandsMetadata
            .FirstOrDefault(x => x.IsHelpCommand);

        if (commandMetadata != null)
            return commandMetadata;

        return commandsMetadata
            .FirstOrDefault(x => string.Equals(x.Name, "help", StringComparison.InvariantCultureIgnoreCase));
    }

    public IEnumerable<CommandMetadata> GetAnonymous()
    {
        return commandsMetadata.Where(x => x.IsEnabled && x.Name == null);
    }

    public void Freeze()
    {
        isFrozen = true;
    }
}