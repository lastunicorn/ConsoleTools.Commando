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

            CommandCollection commandCollection = new();
            Assembly commandoAssembly = typeof(CommandCollection).Assembly;
            commandCollection.LoadFrom(commandoAssembly);
            commandCollection.LoadFrom(assemblies);

            containerBuilder.RegisterInstance(commandCollection).AsSelf().SingleInstance();

            foreach (Type type in commandCollection.GetCommandTypes())
                containerBuilder.RegisterType(type).AsSelf();

            foreach (Type type in commandCollection.GetViewTypes())
                containerBuilder.RegisterType(type).AsSelf();

            containerBuilder.RegisterType<Application>().AsSelf().SingleInstance();
        }
    }
}