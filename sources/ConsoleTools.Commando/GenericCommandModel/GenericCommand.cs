using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ConsoleTools.Commando.CommandMetadataModel;

namespace DustInTheWind.ConsoleTools.Commando.GenericCommandModel;

public class GenericCommand
{
    private List<GenericCommandOption> unusedOptions = new();
    private List<string> unusedOperands = new();

    public string[] UnderlyingArgs { get; init; }

    public string Verb { get; init; }

    public List<GenericCommandOption> Options { get; } = new();

    public List<string> Operands { get; } = new();

    public void Reset()
    {
        unusedOptions = Options.ToList();
        unusedOperands = Operands.ToList();
    }

    public GenericCommandOption GetOptionAndMarkAsUsed(ParameterMetadata parameterMetadata)
    {
        if (parameterMetadata.Name != null)
        {
            GenericCommandOption option = Options
                .FirstOrDefault(x => x.Name == parameterMetadata.Name);

            if (option != null)
            {
                unusedOptions.Remove(option);
                return option;
            }
        }

        if (parameterMetadata.ShortName != 0)
        {
            GenericCommandOption option = Options
                .FirstOrDefault(x => x.Name == parameterMetadata.ShortName.ToString());

            if (option != null)
            {
                unusedOptions.Remove(option);
                return option;
            }
        }

        return null;
    }

    public string GetOperandAndMarkAsUsed(ParameterMetadata parameterMetadata)
    {
        if (parameterMetadata.Order != null)
        {
            int index = parameterMetadata.Order.Value - 1;

            if (index >= 0 && index < Operands.Count)
            {
                string operand = Operands[index];

                unusedOperands.Remove(operand);
                return operand;
            }
        }

        return null;
    }

    public IEnumerable<GenericCommandOption> EnumerateUnusedOptions()
    {
        return unusedOptions;
    }

    public IEnumerable<string> EnumerateUnusedOperands()
    {
        return unusedOperands;
    }
}