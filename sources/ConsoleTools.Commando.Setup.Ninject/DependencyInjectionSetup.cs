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
using DustInTheWind.ConsoleTools.Commando.MetadataModel;
using DustInTheWind.ConsoleTools.Commando.Parsing;
using Ninject;
using ExecutionContext = DustInTheWind.ConsoleTools.Commando.MetadataModel.ExecutionContext;

namespace DustInTheWind.ConsoleTools.Commando.Setup.Ninject;

public static class DependencyInjectionSetup
{
    public static void RegisterCommando(this IKernel kernel, params Assembly[] assemblies)
    {
        RegisterCommando(kernel, typeof(CommandParser), assemblies);
    }

    public static void RegisterCommando(this IKernel kernel, Type commandParserType, params Assembly[] assemblies)
    {
        kernel.Bind<ICommandFactory>().To<CommandFactory>();
        kernel.Bind<ICommandParser>().To(commandParserType);

        ExecutionContext executionContext = new();
        executionContext.LoadFromAssemblyContaining<ExecutionContext>();
        executionContext.LoadFrom(assemblies);

        executionContext.Freeze();

        kernel.Bind<ExecutionContext>().ToConstant(executionContext).InSingletonScope();

        foreach (Type type in executionContext.Commands.GetCommandTypes())
            kernel.Bind(type).ToSelf();

        foreach (Type type in executionContext.Views.GetViewTypes())
            kernel.Bind(type).ToSelf();

        kernel.Bind<Application>().ToSelf().InSingletonScope();
    }
}