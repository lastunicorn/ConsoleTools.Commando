using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando.Analysis;

internal class ParameterMatch
{
    private readonly ParameterMetadata parameterMetadata;
    private readonly CommandArgument commandArgument;
    private readonly CommandArgumentType argumentType;

    public bool IsMatch { get; }

    public bool IsParameterMandatory => parameterMetadata.IsMandatory;

    public string Name => parameterMetadata.Name ?? parameterMetadata.DisplayName ?? parameterMetadata.Order.ToString();

    public ParameterMatch(ParameterMetadata parameterMetadata, CommandRequest commandRequest)
    {
        if (commandRequest == null) throw new ArgumentNullException(nameof(commandRequest));
        this.parameterMetadata = parameterMetadata ?? throw new ArgumentNullException(nameof(parameterMetadata));

        CommandArgument option = commandRequest.GetOptionAndMarkAsUsed(parameterMetadata);

        if (option != null)
        {
            commandArgument = option;
            argumentType = CommandArgumentType.Option;
            IsMatch = true;
            return;
        }

        CommandArgument operand = commandRequest.GetOperandAndMarkAsUsed(parameterMetadata);

        if (operand != null)
        {
            commandArgument = operand;
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
                parameterMetadata.SetValue(consoleCommand, commandArgument.Value);
                break;

            case CommandArgumentType.Operand:
                parameterMetadata.SetValue(consoleCommand, commandArgument.Value);
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