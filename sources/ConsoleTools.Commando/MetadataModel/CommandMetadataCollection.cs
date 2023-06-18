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

public class CommandMetadataCollection
{
    private readonly List<CommandMetadata> commandsMetadata = new();
    private readonly List<ViewMetadata> viewsMetadata = new();
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
            CommandMetadata commandMetadata = new(type);

            if (commandMetadata.CommandKind != CommandKind.None)
            {
                commandsMetadata.Add(commandMetadata);
                continue;
            }
            
            ViewMetadata viewMetadata = new(type);

            if (viewMetadata.IsViewType())
                viewsMetadata.Add(viewMetadata);
        }
    }

    public void Clear()
    {
        if (isFrozen)
            throw new InvalidOperationException();

        commandsMetadata.Clear();
        viewsMetadata.Clear();
    }

    public IEnumerable<Type> GetCommandTypes()
    {
        return commandsMetadata.Select(x => x.Type);
    }

    public IEnumerable<Type> GetViewTypes()
    {
        return viewsMetadata.Select(x => x.Type);
    }

    public IEnumerable<Type> GetViewTypesForCommand(Type viewModelType)
    {
        return viewsMetadata
            .Where(x => x.IsViewFor(viewModelType))
            .Select(x=>x.Type);
    }

    public CommandMetadata GetByName(string commandName)
    {
        return commandsMetadata
            .Where(x => x.IsEnabled && x.Name == commandName)
            .MinBy(x => x.Order);
    }

    public IEnumerable<CommandMetadata> GetNamed()
    {
        return commandsMetadata
            .Where(x => x.IsEnabled && !x.IsHelpCommand && x.Name != null)
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Name)
            .Concat(new[] { GetHelpCommand() });
    }

    public IEnumerable<CommandMetadata> GetAnonymous()
    {
        return commandsMetadata
            .Where(x => x.IsEnabled && x.Name == null)
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Name);
    }

    public CommandMetadata GetHelpCommand()
    {
        return commandsMetadata.FirstOrDefault(x => x.IsHelpCommand);
    }

    public void Freeze()
    {
        isFrozen = true;
    }
}