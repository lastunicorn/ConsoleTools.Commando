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

public class ExecutionContext
{
    private bool isFrozen;

    public CommandMetadataCollection Commands { get; } = new();

    public ViewMetadataCollection Views { get; } = new();

    public void LoadFromCurrentAppDomain()
    {
        if (isFrozen)
            throw new ObjectFrozenException();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        LoadFrom(assemblies);
    }

    public void LoadFromAssemblyContaining<T>()
    {
        Assembly assembly = typeof(T).Assembly;
        LoadFrom(assembly);
    }

    public void LoadFrom(params Assembly[] assemblies)
    {
        if (isFrozen)
            throw new ObjectFrozenException();

        IEnumerable<Type> allTypes = assemblies
            .SelectMany(x => x.GetTypes());

        foreach (Type type in allTypes)
        {
            CommandMetadata commandMetadata = new(type);

            if (commandMetadata.CommandKind != CommandKind.None)
            {
                Commands.Add(commandMetadata);
                continue;
            }

            ViewMetadata viewMetadata = new(type);

            if (viewMetadata.IsViewType())
                Views.Add(viewMetadata);
        }
    }

    public void Clear()
    {
        if (isFrozen)
            throw new ObjectFrozenException();

        Commands.Clear();
        Views.Clear();
    }

    public void Freeze()
    {
        isFrozen = true;

        Commands.Freeze();
        Views.Freeze();
    }
}