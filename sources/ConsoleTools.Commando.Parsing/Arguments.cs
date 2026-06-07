using System.Collections;

namespace DustInTheWind.ConsoleTools.Commando.Parsing;

internal class Arguments : IEnumerable<Argument>
{
    public string[] UnderlyingArgs { get; }

    private readonly List<Argument> arguments = new();

    public int Count => arguments.Count;

    public Argument this[int index]
    {
        get
        {
            bool isValidIndex = index >= 0 && index < arguments.Count;

            return isValidIndex
                ? arguments[index]
                : null;
        }
    }

    public Argument this[string name] => arguments.FirstOrDefault(x => x.Name == name);

    public Arguments(string[] args)
    {
        UnderlyingArgs = args ?? throw new ArgumentNullException(nameof(args));

        IEnumerable<Argument> newArguments = Parse(args);
        arguments.AddRange(newArguments);
    }

    private static IEnumerable<Argument> Parse(IEnumerable<string> args)
    {
        Argument previousArgument = null;

        IEnumerable<Argument> arguments = ExtractArguments(args);

        foreach (Argument argument in arguments)
        {
            if (argument.IsNamedArgument)
            {
                if (previousArgument != null)
                {
                    yield return previousArgument;
                    previousArgument = null;
                }

                if (argument.Value != null)
                    yield return argument;
                else
                    previousArgument = argument;
            }
            else
            {
                if (previousArgument != null)
                {
                    yield return new Argument
                    {
                        Name = previousArgument.Name,
                        Value = argument.Value
                    };

                    previousArgument = null;
                }
                else
                {
                    yield return argument;
                }
            }
        }

        if (previousArgument != null)
            yield return previousArgument;
    }

    private static IEnumerable<Argument> ExtractArguments(IEnumerable<string> args)
    {
        bool forceToBeOperand = false;

        foreach (string arg in args)
        {
            if (arg == "--")
            {
                forceToBeOperand = true;
                continue;
            }

            if (forceToBeOperand)
            {
                yield return new Argument
                {
                    Value = arg,
                    IsForcedToBeAnonymous = true
                };
            }
            else
            {
                ChunkAnalysis chunkAnalysis = new(arg);
                chunkAnalysis.Analyze();

                foreach (Argument argument in chunkAnalysis)
                {
                    yield return argument;
                }
            }
        }
    }

    public IEnumerator<Argument> GetEnumerator()
    {
        return arguments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}