﻿// ConsoleTools.Commando
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

using System.Reflection;
using Autofac;
using DustInTheWind.ConsoleTools.Commando.Autofac.DependencyInjection;

namespace DustInTheWind.ConsoleTools.Commando.Demo
{
    internal class Setup
    {
        public static IContainer ConfigureServices()
        {
            ContainerBuilder containerBuilder = new();

            Assembly presentationAssembly = Assembly.GetExecutingAssembly();
            containerBuilder.RegisterCommando(presentationAssembly);

            return containerBuilder.Build();
        }
    }
}