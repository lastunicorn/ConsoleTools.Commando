using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class ParameterMatch
{
    private readonly ParameterMetadata parameterMetadata;
    private readonly XArgument xArgument;
    private readonly CommandArgumentType argumentType;

    public bool IsMatch { get; }

    public bool IsMandatory => parameterMetadata.IsMandatory;

    public string Name => parameterMetadata.Name ?? parameterMetadata.DisplayName ?? parameterMetadata.Order.ToString();

    public ParameterMatch(ParameterMetadata parameterMetadata, UnusedArguments unusedArguments)
    {
        if (unusedArguments == null) throw new ArgumentNullException(nameof(unusedArguments));
        this.parameterMetadata = parameterMetadata ?? throw new ArgumentNullException(nameof(parameterMetadata));

        XArgument option = unusedArguments.PopOption(parameterMetadata);

        if (option != null)
        {
            xArgument = option;
            argumentType = CommandArgumentType.Option;
            IsMatch = true;
            return;
        }

        XArgument operand = unusedArguments.PopOperand(parameterMetadata);

        if (operand != null)
        {
            xArgument = operand;
            argumentType = CommandArgumentType.Operand;
            IsMatch = true;
            return;
        }
    }


    public void SetParameter(object consoleCommand)
    {
        if (!IsMatch)
            throw new Exception("The argument does not match the parameter. Value cannot be set.");

        SetParameterInternal(consoleCommand);
    }

    private void SetParameterInternal(object consoleCommand)
    {
        switch (argumentType)
        {
            case CommandArgumentType.Unknown:
                throw new Exception($"Error setting the parameter value. Parameter: {parameterMetadata.Name}.");

            case CommandArgumentType.Option:
                parameterMetadata.SetValue(consoleCommand, xArgument.Value);
                break;

            case CommandArgumentType.Operand:
                parameterMetadata.SetValue(consoleCommand, xArgument.Value);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override string ToString()
    {
        return $"IsMatch: {IsMatch}; Parameter: {parameterMetadata}";
    }
}