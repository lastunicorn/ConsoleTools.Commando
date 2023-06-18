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

namespace DustInTheWind.ConsoleTools.Commando.Setup.Microsoft;

internal class CommandFactory : ICommandFactory
{
    private readonly IServiceProvider serviceProvider;

    public CommandFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IConsoleCommand Create(Type commandType)
    {
        if (commandType == null) throw new ArgumentNullException(nameof(commandType));

        bool isCommandType = typeof(IConsoleCommand).IsAssignableFrom(commandType);
        if (!isCommandType)
            throw new TypeIsNotCommandException(commandType);

        return (IConsoleCommand)serviceProvider.GetService(commandType);
    }

    public object CreateView(Type viewType)
    {
        return serviceProvider.GetService(viewType);
    }
}