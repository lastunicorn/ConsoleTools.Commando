using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class UnusedArguments
{
    private readonly List<XArgument> arguments;

    public UnusedArguments(IEnumerable<XArgument> arguments)
    {
        this.arguments = arguments.ToList();
    }

    public XArgument PopOption(ParameterMetadata parameterMetadata)
    {
        if (parameterMetadata.Name != null)
        {
            XArgument xArgument = arguments
                .Where(x => x.Name != null)
                .FirstOrDefault(x => x.Name == parameterMetadata.Name);

            if (xArgument != null)
            {
                arguments.Remove(xArgument);
                return xArgument;
            }
        }

        if (parameterMetadata.ShortName != 0)
        {
            XArgument xArgument = arguments
                .Where(x => x.Name != null)
                .FirstOrDefault(x => x.Name == parameterMetadata.ShortName.ToString());

            if (xArgument != null)
            {
                arguments.Remove(xArgument);
                return xArgument;
            }
        }

        return null;
    }

    public XArgument PopOperand(ParameterMetadata parameterMetadata)
    {
        if (parameterMetadata.Order != null)
        {
            int index = parameterMetadata.Order.Value - 1;

            if (index >= 0)
            {
                XArgument xArgument = arguments
                    .Where(x => x.Name == null)
                    .Skip(index)
                    .FirstOrDefault();

                if (xArgument != null)
                {
                    arguments.Remove(xArgument);
                    return xArgument;
                }
            }
        }

        return null;
    }

    public IEnumerable<XArgument> EnumerateOptions()
    {
        return arguments
            .Where(x => x.Name != null);
    }

    public IEnumerable<string> EnumerateOperands()
    {
        return arguments
            .Where(x => x.Name == null)
            .Select(x => x.Value);
    }
}