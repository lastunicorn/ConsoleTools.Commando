using System;

namespace DustInTheWind.ConsoleTools.Commando.Demo;

public abstract partial class ViewBase<TCommand> : IView<TCommand>
    where TCommand : ICommand
{
    public void WriteLine()
    {
        Console.WriteLine();
    }

    public void WriteHorizontalLine(int margin = 0)
    {
        for (int i = 0; i < margin; i++)
            Console.WriteLine();

        Console.WriteLine(new string('-', 79));

        for (int i = 0; i < margin; i++)
            Console.WriteLine();
    }

    public void WriteHorizontalLine(int topMargin, int bottomMargin)
    {
        for (int i = 0; i < topMargin; i++)
            Console.WriteLine();

        Console.WriteLine(new string('-', 79));

        for (int i = 0; i < bottomMargin; i++)
            Console.WriteLine();
    }

    private static void WriteWithColor(ConsoleColor color, string text)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = oldColor;
    }

    private static void WriteLineWithColor(ConsoleColor color, string text)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = oldColor;
    }

    public abstract void Display(TCommand command);
}