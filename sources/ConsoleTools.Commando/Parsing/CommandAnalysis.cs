using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ConsoleTools.Commando.GenericCommandModel;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class CommandAnalysis
{
    private readonly Arguments arguments;

    public CommandAnalysis(string[] args)
    {
        if (args == null) throw new ArgumentNullException(nameof(args));

        arguments = new Arguments(args);
    }

    public GenericCommand Analyze()
    {
        Argument verbArgument = GetVerb();

        GenericCommand genericCommand = new()
        {
            UnderlyingArgs = arguments.UnderlyingArgs,
            Verb = verbArgument?.Value
        };

        IEnumerable<GenericCommandOption> options = GetOptions();
        genericCommand.Options.AddRange(options);

        IEnumerable<string> operands = GetOperands(verbArgument);
        genericCommand.Operands.AddRange(operands);

        return genericCommand;
    }

    public Argument GetVerb()
    {
        if (arguments.Count == 0)
            return null;

        Argument verbArgument = arguments[0];

        if (verbArgument.Type != ArgumentType.Ordinal)
            return null;

        return verbArgument;
    }

    private IEnumerable<GenericCommandOption> GetOptions()
    {
        return arguments
            .Where(x => x.Type == ArgumentType.Named)
            .Select(x => new GenericCommandOption(x.Name, x.Value));
    }

    private IEnumerable<string> GetOperands(Argument verbArgument)
    {
        IEnumerable<Argument> query = arguments
            .Where(x => x.Type == ArgumentType.Ordinal);

        bool skipFirsArgument = !string.IsNullOrEmpty(verbArgument?.Value);

        if (skipFirsArgument)
            query = query.Skip(1);

        return query.Select(x => x.Value);
    }
}