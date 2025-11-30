using InventorySystem.Core;
using InventorySystem.Core.Enums;
using InventorySystem.Core.Interfaces;

namespace InventorySystem.Items
{
    public class Weapon : Item, IUsable, IUpgradable
    {
        public int Damage { get; private set; }
        public int Level { get; private set; } = 1;
        public int MaxLevel { get; } = 3;

        public Weapon(string id, string name, int damage, ItemRarity rarity = ItemRarity.Common) : base(id, name, ItemType.Weapon, rarity, damage * 10)
        {
            Damage = damage;
        }

        public void Upgrade()
        {
            Level++;
            Damage = (int)(Damage * 1.2);
        }

        public bool CanUpgrade() => Level < MaxLevel;
        
        public void Use()
        {
            if (!CanUse()) throw new InvalidOperationException("Cannot use weapon");
            Console.WriteLine($"Attacking with {Name} for {Damage} damage");
        }

        public bool CanUse() => true;
    }
}