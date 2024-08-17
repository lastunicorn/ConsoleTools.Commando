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
using DustInTheWind.ConsoleTools.Commando.Parsing;
using Microsoft.Extensions.DependencyInjection;
using ExecutionContext = DustInTheWind.ConsoleTools.Commando.MetadataModel.ExecutionContext;

namespace DustInTheWind.ConsoleTools.Commando.Setup.Microsoft;

public static class DependencyInjectionSetup
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

        ExecutionContext executionContext = new();
        executionContext.LoadFromAssemblyContaining<ExecutionContext>();
        executionContext.LoadFrom(assemblies);

        executionContext.Freeze();

        serviceCollection.AddSingleton(executionContext);

        foreach (Type type in executionContext.Commands.GetCommandTypes())
            serviceCollection.AddTransient(type);

        foreach (Type type in executionContext.Views.GetViewTypes())
            serviceCollection.AddTransient(type);

        serviceCollection.AddSingleton<Application>();
    }
}