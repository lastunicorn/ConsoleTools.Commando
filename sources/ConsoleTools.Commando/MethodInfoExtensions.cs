﻿// ConsoleTools.Commando
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

namespace DustInTheWind.ConsoleTools.Commando;

public static class MethodInfoExtensions
{
    public static async Task<object> InvokeAsync(this MethodInfo methodInfo, object obj, params object[] parameters)
    {
        Task task = (Task)methodInfo.Invoke(obj, parameters);

        await task.ConfigureAwait(false);

        PropertyInfo resultProperty = task.GetType().GetProperty("Result");
        return resultProperty.GetValue(task);
    }
}