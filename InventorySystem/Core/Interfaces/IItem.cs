using InventorySystem.Core.Enums;

namespace InventorySystem.Core.Interfaces
{
    public interface IItem
    {
        string Id { get; }
        string Name { get; }
        ItemType Type { get; }
        ItemRarity Rarity { get; }
        int Value { get; }
    }
}