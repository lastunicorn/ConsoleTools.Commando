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
using Autofac;
using DustInTheWind.ConsoleTools.Commando.Parsing;
using ExecutionContext = DustInTheWind.ConsoleTools.Commando.MetadataModel.ExecutionContext;

namespace DustInTheWind.ConsoleTools.Commando.Setup.Autofac;

public static class DependencyContainerSetup
{
    public static void RegisterCommando(this ContainerBuilder containerBuilder, params Assembly[] assemblies)
    {
        RegisterCommando(containerBuilder, typeof(CommandParser), assemblies);
    }

    public static void RegisterCommando(this ContainerBuilder containerBuilder, Type commandParserType, params Assembly[] assemblies)
    {
        containerBuilder.RegisterType<EnhancedConsole>().AsSelf();

        containerBuilder.RegisterType<CommandRouter>().AsSelf();
        containerBuilder.RegisterType<CommandFactory>().As<ICommandFactory>();
        containerBuilder.RegisterType(commandParserType).As<ICommandParser>();

        ExecutionContext executionContext = new();
        executionContext.LoadFromAssemblyContaining<ExecutionContext>();
        executionContext.LoadFrom(assemblies);

        executionContext.Freeze();

        containerBuilder.RegisterInstance(executionContext).AsSelf().SingleInstance();

        foreach (Type type in executionContext.Commands.GetCommandTypes())
            containerBuilder.RegisterType(type).AsSelf();

        foreach (Type type in executionContext.Views.GetViewTypes())
            containerBuilder.RegisterType(type).AsSelf();

        containerBuilder.RegisterType<Application>().AsSelf().SingleInstance();
    }
}