using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Core.Interfaces;
using InventorySystem.Core.Enums;

namespace InventorySystem
{
    public class Inventory
    {
        private readonly List<IItem> _items;
        private readonly int _capacity;
        public IReadOnlyList<IItem> Items => _items.AsReadOnly();
        public int Capacity => _capacity;
        public bool IsFull => _items.Count >= _capacity;

        public Inventory(int capacity = 20)
        {
            if (capacity <= 0) throw new ArgumentException("Capacity must be greater than 0", nameof(capacity));
            _capacity = capacity;
            _items = new List<IItem>();
        }

        public bool AddItem(IItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (IsFull) return false;
            if (_items.Any(i => i.Id == item.Id)) return false;
            _items.Add(item);
            return true;
        }

        public IItem GetItem(string itemId)
        {
            return _items.FirstOrDefault(i => i.Id == itemId);
        }

        public bool RemoveItem(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId)) throw new ArgumentException("Item ID cannot be null or empty", nameof(itemId));
            var item = GetItem(itemId);
            if (item == null) return false;
            return _items.Remove(item);
        }

        public bool RemoveItem(IItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return RemoveItem(item.Id);
        }

        public IEnumerable<IItem> GetItemsByType(ItemType type)
        {
            return _items.Where(item => item.Type == type);
        }

        public IEnumerable<IItem> GetItemsByRarity(ItemRarity rarity)
        {
            return _items.Where(item => item.Rarity == rarity);
        }

        public bool UseItem(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null || item is not IUsable usable) return false;
            if (!usable.CanUse()) return false;
            usable.Use();
            return true;
        }

        public bool UpgradeItem(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null || item is not IUpgradable upgradable) return false;
            if (!upgradable.CanUpgrade()) return false;
            upgradable.Upgrade();
            return true;
        }

        public string GetInventoryInfo()
        {
            var info = $"Inventory: {_items.Count}/{_capacity} items\n";
            var groups = _items.GroupBy(item => item.Type).OrderBy(g => g.Key.ToString());
            foreach (var group in groups) info += $"{group.Key}: {group.Count()}\n";
            var upgradableItems = _items.OfType<IUpgradable>().ToList();
            if (upgradableItems.Any())
            {
                info += $"\nUpgradable items: {upgradableItems.Count}";
                foreach (var upgradable in upgradableItems)
                {
                    if (upgradable is IItem item)
                    {
                        info += $"\n- {item.Name} (Level {upgradable.Level}/{upgradable.MaxLevel})";
                    }
                }
            }
            return info;
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}