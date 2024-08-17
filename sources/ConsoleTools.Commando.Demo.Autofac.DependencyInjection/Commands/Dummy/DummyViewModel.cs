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

namespace DustInTheWind.ConsoleTools.Commando.Demo.Autofac.DependencyInjection.Commands.Dummy;

internal class DummyViewModel
{
    public string Text { get; set; }

    public bool Flag { get; set; }

    public bool Flag2 { get; set; }

    public int IntegerNumber { get; set; }

    public float RealNumber { get; set; }

    public char Character { get; set; }

    public string Param1 { get; set; }

    public int? Param2 { get; set; }
}