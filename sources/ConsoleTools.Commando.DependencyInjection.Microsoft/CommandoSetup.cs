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
using System.Reflection;
using DustInTheWind.ConsoleTools.Commando.CommandMetadataModel;
using DustInTheWind.ConsoleTools.Commando.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.ConsoleTools.Commando.DependencyInjection.Microsoft;

public static class CommandoSetup
{
    public static void AddCommando(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        AddCommando(serviceCollection, typeof(CommandParser), assemblies);
    }

    public static void AddCommando(this IServiceCollection serviceCollection, Type commandParserType, params Assembly[] assemblies)
    {
        serviceCollection.AddTransient<EnhancedConsole>();

        serviceCollection.AddTransient<CommandRouter>();
        serviceCollection.AddTransient<ICommandFactory, CommandFactory>();
        serviceCollection.AddTransient(typeof(ICommandParser), commandParserType);

        CommandMetadataCollection commandMetadataCollection = new();
        Assembly commandoAssembly = typeof(CommandMetadataCollection).Assembly;
        commandMetadataCollection.LoadFrom(commandoAssembly);
        commandMetadataCollection.LoadFrom(assemblies);

        commandMetadataCollection.Freeze();

        serviceCollection.AddSingleton(commandMetadataCollection);

        foreach (Type type in commandMetadataCollection.GetCommandTypes())
            serviceCollection.AddTransient(type);

        foreach (Type type in commandMetadataCollection.GetViewTypes())
            serviceCollection.AddTransient(type);

        serviceCollection.AddSingleton<Application>();
    }
}