using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class TextCommandAnalysis
{
    private readonly string[] args;

    public TextCommandAnalysis(string[] args)
    {
        this.args = args ?? throw new ArgumentNullException(nameof(args));
    }

    public XCommand Analyze()
    {
        XCommand xCommand = new()
        {
            UnderlyingArgs = args
        };

        IEnumerable<Argument> arguments = EnumerateArguments();
        int index = 0;
        
        foreach (Argument argument in arguments)
        {
            bool isCommandName = index == 0 && argument.IsAnonymousArgument && !argument.IsForcedToBeAnonymous;
            if (isCommandName)
            {
                xCommand.Name = argument.Value;
                continue;
            }

            bool isCommandAction = index == 1 && xCommand.Name != null && argument.IsAnonymousArgument && !argument.IsForcedToBeAnonymous;
            if (isCommandAction)
            {
                xCommand.Action = argument.Value;
                continue;
            }

            XArgument xArgument = new(argument.Name, argument.Value);
            xCommand.AddParameter(xArgument);

            index++;
        }

        return xCommand;
    }

    private IEnumerable<Argument> EnumerateArguments()
    {
        using ArgumentEnumerator argumentEnumerator = new(args);

        while (argumentEnumerator.MoveNext())
            yield return argumentEnumerator.Current;
    }
}