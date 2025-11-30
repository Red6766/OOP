using System;
using InventorySystem.Core.Enums;
using InventorySystem.Core.Interfaces;
using InventorySystem.Items;

namespace InventorySystem
{
    public class Builder
    {
        private string _id;
        private string _name;
        private ItemType _type;
        private ItemRarity _rarity = ItemRarity.Common;
        private int _damage;
        private int _defense;
        private int _healingPower;

        public Builder WithId(string id)
        {
            _id = id;
            return this;
        }

        public Builder WithName(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            return this;
        }

        public Builder WithType(ItemType type)
        {
            _type = type;
            return this;
        }

        public Builder WithRarity(ItemRarity rarity)
        {
            _rarity = rarity;
            return this;
        }

        public Builder WithDamage(int damage)
        {
            _damage = damage > 0 ? damage : throw new ArgumentException("Damage must be positive");
            return this;
        }

        public Builder WithDefense(int defense)
        {
            _defense = defense >= 0 ? defense : throw new ArgumentException("Defense cannot be negative");
            return this;
        }

        public Builder WithHealingPower(int healingPower)
        {
            _healingPower = healingPower > 0 ? healingPower : throw new ArgumentException("Healing power must be positive");
            return this;
        }

        public IItem Build()
        {
            var itemId = _id ?? Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(_name)) throw new InvalidOperationException("Name must be specified");
            IItem item = _type switch
            {
                ItemType.Weapon => new Weapon(itemId, _name, _damage, _rarity),
                ItemType.Armor => new Armor(itemId, _name, _defense, _rarity),
                ItemType.Potion => new HealthPotion(itemId, _name, _healingPower),
                ItemType.QuestItem => new QuestItem(itemId, _name),
                _ => throw new InvalidOperationException($"Unsupported item type: {_type}")
            };
            Reset();
            return item;
        }

        private void Reset()
        {
            _id = null;
            _name = null;
            _type = default;
            _rarity = ItemRarity.Common;
            _damage = 0;
            _defense = 0;
            _healingPower = 0;
        }
    }
}