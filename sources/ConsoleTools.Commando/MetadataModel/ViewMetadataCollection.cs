using System.Collections.ObjectModel;

namespace DustInTheWind.ConsoleTools.Commando.MetadataModel;

public class ViewMetadataCollection : Collection<ViewMetadata>
{
    public bool IsFrozen { get; private set; }

    protected override void InsertItem(int index, ViewMetadata item)
    {
        if (IsFrozen)
            throw new ObjectFrozenException();

        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, ViewMetadata item)
    {
        if (IsFrozen)
            throw new ObjectFrozenException();

        base.SetItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        if (IsFrozen)
            throw new ObjectFrozenException();

        base.RemoveItem(index);
    }

    protected override void ClearItems()
    {
        if (IsFrozen)
            throw new ObjectFrozenException();

        base.ClearItems();
    }

    public IEnumerable<Type> GetViewTypes()
    {
        return Items.Select(x => x.Type);
    }

    public IEnumerable<Type> GetViewTypesForModel(Type viewModelType)
    {
        return Items
            .Where(x => x.IsViewFor(viewModelType))
            .Select(x => x.Type);
    }

    public void Freeze()
    {
        IsFrozen = true;
    }
}