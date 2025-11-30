using System;
using InventorySystem.Core;
using InventorySystem.Core.Enums;
using InventorySystem.Core.Interfaces;

namespace InventorySystem.Items
{
    public class HealthPotion : Item, IUsable
    {
        public int HealthPower { get; }
        public int UsesRemaining { get; private set; } = 1;

        public HealthPotion(string id, string name, int HealthPower) : base(id, name, ItemType.Potion, ItemRarity.Common, HealthPower * 2)
        {
            HealthPower = HealthPower;
        }

        public void Use()
        {
            if (!CanUse()) throw new InvalidOperationException("Cannot use potion");
            UsesRemaining--;
        }

        public bool CanUse() => UsesRemaining > 0;
    }
}