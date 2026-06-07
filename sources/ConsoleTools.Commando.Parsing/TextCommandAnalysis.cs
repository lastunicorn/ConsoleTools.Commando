using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class TextCommandAnalysis
{
    private readonly Arguments arguments;

    public TextCommandAnalysis(string[] args)
    {
        if (args == null) throw new ArgumentNullException(nameof(args));

        arguments = new Arguments(args);
    }

    public CommandRequest Analyze()
    {
        CommandRequest commandRequest = new()
        {
            UnderlyingArgs = arguments.UnderlyingArgs
        };

        bool isFirst = true;

        foreach (Argument argument in arguments)
        {
            if (isFirst)
            {
                isFirst = false;

                if (argument.IsAnonymousArgument && !argument.IsForcedToBeAnonymous)
                {
                    commandRequest.CommandName = argument.Value;
                    continue;
                }
            }

            CommandArgument commandArgument = new(argument.Name, argument.Value);
            commandRequest.AddParameter(commandArgument);
        }

        return commandRequest;
    }
}