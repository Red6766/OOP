using System;
using InventorySystem.Core.Enums;
using InventorySystem.Core.Interfaces;

namespace InventorySystem.Core
{
    public abstract class Item : IItem
    {
        public string Id { get; }
        public string Name { get; protected set; }
        public ItemType Type { get; protected set; }
        public ItemRarity Rarity { get; protected set; }
        public int Value { get; protected set; }

        protected Item(string id, string name, ItemType type, ItemRarity rarity, int value)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
            Rarity = rarity;
            Value = value >= 0 ? value : throw new ArgumentException("Value cannot be negative");
        }

        public override bool Equals(object obj)
        {
            return obj is Item item && Id == item.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}