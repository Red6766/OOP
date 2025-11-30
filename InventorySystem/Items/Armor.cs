using System;
using InventorySystem.Core;
using InventorySystem.Core.Enums;
using InventorySystem.Core.Interfaces;

namespace InventorySystem.Items
{
    public class Armor : Item, IUsable, IUpgradable
    {
        public int Defense { get; private set; }
        public int Level { get; private set; } = 1;
        public int MaxLevel { get; } = 3;

        public Armor(string id, string name, int defense, ItemRarity rarity = ItemRarity.Common) : base(id, name, ItemType.Armor, rarity, defense * 8)
        {
            Defense = defense;
        }

        public void Upgrade()
        {
            Level++;
            Defense = (int)(Defense * 1.15);
        }

        public bool CanUpgrade() => Level < MaxLevel;
        
        public void Use()
        {
            if (!CanUse()) throw new InvalidOperationException("Cannot use armor");
            Console.WriteLine($"{Name} provides {Defense} defense");
        }

        public bool CanUse() => true;
    }
}