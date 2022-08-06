// ConsoleTools.Commando
// Copyright (C) 2022 Dust in the Wind
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
using Autofac;

namespace DustInTheWind.ConsoleTools.Commando.Autofac.DependencyInjection
{
    public static class CommandoSetup
    {
        public static void RegisterCommando(this ContainerBuilder containerBuilder, params Assembly[] assemblies)
        {
            containerBuilder.RegisterType<CommandRouter>().AsSelf();
            containerBuilder.RegisterType<CommandFactory>().As<ICommandFactory>();

            AvailableCommands availableCommands = new();
            Assembly commandoAssembly = typeof(AvailableCommands).Assembly;
            availableCommands.LoadFrom(commandoAssembly);
            availableCommands.LoadFrom(assemblies);

            containerBuilder.RegisterInstance(availableCommands).AsSelf();

            foreach (Type type in availableCommands.GetCommandTypes())
                containerBuilder.RegisterType(type).AsSelf();

            foreach (Type type in availableCommands.GetViewTypes())
                containerBuilder.RegisterType(type).AsSelf();

            containerBuilder.RegisterType<Application>().AsSelf();
        }
    }
}