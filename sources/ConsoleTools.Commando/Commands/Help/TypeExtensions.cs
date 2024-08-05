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

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

internal static class TypeExtensions
{
    public static bool IsText(this Type type)
    {
        return type == typeof(string);
    }

    public static bool IsListOfTexts(this Type type)
    {
        return type.GetInterfaces().Contains(typeof(IEnumerable<string>));
    }

    public static bool IsInteger(this Type type)
    {
        return type == typeof(int) ||
               type == typeof(uint) ||
               type == typeof(long) ||
               type == typeof(ulong) ||
               type == typeof(short) ||
               type == typeof(ushort);
    }

    public static bool IsReal(this Type type)
    {
        return type == typeof(float) ||
               type == typeof(double);
    }

    public static bool IsNumber(this Type type)
    {
        return IsInteger(type) || IsReal(type);
    }

    public static bool IsListOfIntegers(this Type type)
    {
        Type[] interfaces = type.GetInterfaces();

        return interfaces.Contains(typeof(IEnumerable<int>)) ||
               interfaces.Contains(typeof(IEnumerable<uint>)) ||
               interfaces.Contains(typeof(IEnumerable<long>)) ||
               interfaces.Contains(typeof(IEnumerable<ulong>)) ||
               interfaces.Contains(typeof(IEnumerable<short>)) ||
               interfaces.Contains(typeof(IEnumerable<ushort>));
    }

    public static bool IsListOfReal(this Type type)
    {
        Type[] interfaces = type.GetInterfaces();

        return interfaces.Contains(typeof(IEnumerable<float>)) ||
               interfaces.Contains(typeof(IEnumerable<double>));
    }

    public static bool IsListOfNumbers(this Type type)
    {
        return IsListOfIntegers(type) || IsListOfReal(type);
    }

    public static bool IsBoolean(this Type type)
    {
        return type == typeof(bool);
    }

    public static bool IsCharacter(this Type type)
    {
        return type == typeof(char);
    }

    public static bool IsNullable(this Type type)
    {
        if (!type.IsGenericType)
            return false;

        Type genericType = type.GetGenericTypeDefinition();
        return genericType == typeof(Nullable<>);
    }

    public static bool IsNullable(this Type type, out Type underlyingType)
    {
        if (!type.IsGenericType)
        {
            underlyingType = null;
            return false;
        }

        Type genericType = type.GetGenericTypeDefinition();
        underlyingType = type.GetGenericArguments().First();
        return genericType == typeof(Nullable<>);
    }

    public static string ToUserFriendlyName(this Type type)
    {
        if (type.IsText())
            return "text";

        if (type.IsInteger())
            return "integer";

        if (type.IsReal())
            return "real";

        if (type.IsListOfIntegers())
            return "list-of-integers";

        if (type.IsListOfReal())
            return "list-of-real";

        if (type.IsListOfTexts())
            return "list-of-texts";

        if (type.IsBoolean())
            return "flag";

        if (type.IsCharacter())
            return "character";

        if (type.IsNullable(out Type underlyingType))
            return underlyingType.ToUserFriendlyName();

        return type.Name;
    }
}