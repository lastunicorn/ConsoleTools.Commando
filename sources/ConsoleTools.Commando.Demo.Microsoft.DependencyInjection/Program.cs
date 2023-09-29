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

using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.ConsoleTools.Commando.Demo.Microsoft.DependencyInjection;

/// <summary>
/// Note: The suggested setup approach is the one using <see cref="DustInTheWind.ConsoleTools.Commando.Setup.Microsoft.ApplicationBuilder"/>.
/// </summary>
internal class Program
{
    private static async Task Main(string[] args)
    {
        IServiceProvider serviceProvider = Setup.ConfigureServices();

        using IServiceScope serviceScope = serviceProvider.CreateScope();

        Application application = serviceScope.ServiceProvider.GetService<Application>();
        await application.RunAsync(args);
    }
}