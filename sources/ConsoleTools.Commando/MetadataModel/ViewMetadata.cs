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

namespace DustInTheWind.ConsoleTools.Commando.MetadataModel;

public class ViewMetadata
{
    public Type Type { get; }

    public ViewMetadata(Type type)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public bool IsViewType()
    {
        if (Type.IsAbstract)
            return false;

        Type[] interfaceTypes = Type.GetInterfaces();

        foreach (Type interfaceType in interfaceTypes)
        {
            bool isGenericType = interfaceType.IsGenericType;

            if (!isGenericType)
                continue;

            Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();

            return genericTypeDefinition == typeof(IView<>);
        }

        return false;
    }

    public bool IsViewFor(Type viewModelType)
    {
        IEnumerable<Type> interfaceTypes = Type.GetInterfaces();

        foreach (Type interfaceType in interfaceTypes)
        {
            Type[] genericArgumentTypes = interfaceType.GetGenericArguments();

            if (genericArgumentTypes.Length != 1)
                continue;

            if (genericArgumentTypes[0] != viewModelType)
                continue;

            return true;
        }

        return false;
    }
}