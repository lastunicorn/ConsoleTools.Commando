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

using System.Collections.ObjectModel;

namespace DustInTheWind.ConsoleTools.Commando.MetadataModel;

public class CommandMetadataCollection : Collection<CommandMetadata>
{
    public bool IsFrozen { get; private set; }

    protected override void InsertItem(int index, CommandMetadata item)
    {
        if (IsFrozen)
            throw new ObjectFrozenException();

        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, CommandMetadata item)
    {
        if (IsFrozen)
            throw new ObjectFrozenException();

        base.SetItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        if (IsFrozen)
            throw new ObjectFrozenException();

        base.RemoveItem(index);
    }

    protected override void ClearItems()
    {
        if (IsFrozen)
            throw new ObjectFrozenException();

        base.ClearItems();
    }

    public IEnumerable<Type> GetCommandTypes()
    {
        return Items.Select(x => x.Type);
    }

    public CommandMetadata GetByName(string commandName)
    {
        return Items
            .Where(x => x.IsEnabled && x.Name == commandName)
            .MinBy(x => x.Order);
    }

    public IEnumerable<CommandMetadata> GetAllByName(string commandName)
    {
        return Items
            .Where(x => x.IsEnabled && x.Name == commandName)
            .OrderBy(x => x.Order);
    }

    public IEnumerable<CommandMetadata> GetNamed()
    {
        return Items
            .Where(x => x.IsEnabled && !x.IsHelpCommand && x.Name != null)
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Name)
            .Concat(new[] { GetHelpCommand() });
    }

    public IEnumerable<CommandMetadata> GetAllAnonymous()
    {
        return Items
            .Where(x => x.IsEnabled && x.Name == null)
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Name);
    }

    public CommandMetadata GetHelpCommand()
    {
        return Items.FirstOrDefault(x => x.IsHelpCommand);
    }

    public void Freeze()
    {
        IsFrozen = true;
    }
}