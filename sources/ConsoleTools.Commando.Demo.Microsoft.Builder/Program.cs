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

using DustInTheWind.ConsoleTools.Commando.Demo.Microsoft.Builder.Commands.Dummy;
using DustInTheWind.ConsoleTools.Commando.Setup.Microsoft;

namespace DustInTheWind.ConsoleTools.Commando.Demo.Microsoft.Builder;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Application application = ApplicationBuilder.Create()
            .RegisterCommandsFromAssemblyContaining(typeof(DummyCommand))
            .HandleExceptions(HandleUnhandledApplicationException)
            .Build();

        await application.RunAsync(args);
    }

    /// <summary>
    /// Optional.
    /// If this method is provided, it allows the consumer to take specific actions when an
    /// unhandled exception is thrown during the execution.
    /// </summary>
    private static void HandleUnhandledApplicationException(object sender, UnhandledApplicationExceptionEventArgs e)
    {
#if DEBUG
        CustomConsole.WriteLineError(e.Exception);
#else
        CustomConsole.WriteLineError(e.Exception.Message);
#endif

        e.IsHandled = true;
    }
}