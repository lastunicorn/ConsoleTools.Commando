﻿// ConsoleTools.Commando
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

using DustInTheWind.ConsoleTools.Commando.MetadataModel;
using Ninject;

namespace DustInTheWind.ConsoleTools.Commando.Setup.Ninject;

internal class CommandFactory : ICommandFactory
{
    private readonly IKernel context;

    public CommandFactory(IKernel context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public object Create(CommandMetadata commandMetadata)
    {
        switch (commandMetadata.CommandKind)
        {
            case CommandKind.None:
                throw new TypeIsNotCommandException(commandMetadata.Type);

            case CommandKind.WithoutResult:
            case CommandKind.WithResult:
                return context.Get(commandMetadata.Type);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public object CreateView(Type viewType)
    {
        return context.Get(viewType);
    }
}