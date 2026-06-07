namespace DustInTheWind.ConsoleTools.Commando.MetadataModel;

public class ViewMetadata
{
    public Type Type { get; }

    public ViewMetadata(Type type)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public bool IsViewType()
    {
        if (Type.IsAbstract)
            return false;

        Type[] interfaceTypes = Type.GetInterfaces();

        foreach (Type interfaceType in interfaceTypes)
        {
            bool isGenericType = interfaceType.IsGenericType;

            if (!isGenericType)
                continue;

            Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();

            return genericTypeDefinition == typeof(IView<>);
        }

        return false;
    }

    public bool IsViewFor(Type viewModelType)
    {
        IEnumerable<Type> interfaceTypes = Type.GetInterfaces();

        foreach (Type interfaceType in interfaceTypes)
        {
            Type[] genericArgumentTypes = interfaceType.GetGenericArguments();

            if (genericArgumentTypes.Length != 1)
                continue;

            if (genericArgumentTypes[0] != viewModelType)
                continue;

            return true;
        }

        return false;
    }
}