using System;

namespace DustInTheWind.ConsoleTools.Commando;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CommandParameterAttribute : Attribute
{
    public bool IsOptional { get; set; }

    public string Description { get; set; }
}