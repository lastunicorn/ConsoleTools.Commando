using System.Reflection;

namespace DustInTheWind.ConsoleTools.Commando.Metadata;

public class MetadataContext
{
    private bool isFrozen;

    public CommandMetadataCollection Commands { get; } = new();

    public ViewMetadataCollection Views { get; } = new();

    public void LoadFromCurrentAppDomain()
    {
        if (isFrozen)
            throw new ObjectFrozenException();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        LoadFrom(assemblies);
    }

    public void LoadFromAssemblyContaining<T>()
    {
        Assembly assembly = typeof(T).Assembly;
        LoadFrom(assembly);
    }

    public void LoadFrom(params Assembly[] assemblies)
    {
        if (isFrozen)
            throw new ObjectFrozenException();

        IEnumerable<Type> allTypes = assemblies
            .SelectMany(x => x.GetTypes());

        foreach (Type type in allTypes)
        {
            CommandMetadata commandMetadata = new(type);

            if (commandMetadata.CommandKind != CommandKind.None)
            {
                Commands.Add(commandMetadata);
                continue;
            }

            ViewMetadata viewMetadata = new(type);

            if (viewMetadata.IsViewType())
                Views.Add(viewMetadata);
        }
    }

    public void Clear()
    {
        if (isFrozen)
            throw new ObjectFrozenException();

        Commands.Clear();
        Views.Clear();
    }

    public void Freeze()
    {
        isFrozen = true;

        Commands.Freeze();
        Views.Freeze();
    }
}