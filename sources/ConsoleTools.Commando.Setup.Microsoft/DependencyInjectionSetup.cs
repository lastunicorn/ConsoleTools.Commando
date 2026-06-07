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

using System.Reflection;
using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Parsing;
using DustInTheWind.ConsoleTools.Commando.Routing;
using DustInTheWind.ConsoleTools.Commando.Syntax;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.ConsoleTools.Commando.Setup.Microsoft;

public static class DependencyInjectionSetup
{
    public static void AddCommando(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        AddCommando(serviceCollection, typeof(CliSyntax), assemblies);
    }

    public static void AddCommando(this IServiceCollection serviceCollection, Type commandParserType, params Assembly[] assemblies)
    {
        serviceCollection.AddTransient<EnhancedConsole>();

        serviceCollection.AddTransient<CommandRouter>();
        serviceCollection.AddTransient<ICommandFactory, CommandFactory>();
        serviceCollection.AddTransient(typeof(ICliSyntax), commandParserType);

        MetadataContext metadataContext = new();
        metadataContext.LoadFromAssemblyContaining<MetadataContext>();
        metadataContext.LoadFrom(assemblies);

        metadataContext.Freeze();

        serviceCollection.AddSingleton(metadataContext);

        foreach (Type type in metadataContext.Commands.GetCommandTypes())
            serviceCollection.AddTransient(type);

        foreach (Type type in metadataContext.Views.GetViewTypes())
            serviceCollection.AddTransient(type);

        serviceCollection.AddSingleton<Application>();
    }
}