using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.Models;

namespace FoodDelivery.Services
{
    public sealed class MenuService
    {
        // Singleton паттерн 1
        private static readonly Lazy<MenuService> _instance = new Lazy<MenuService>(() => new MenuService());
        public static MenuService Instance => _instance.Value;

        private readonly List<MenuItem> _menuItems;

        private MenuService()
        {
            _menuItems = new List<MenuItem>();
        }

        public void AddMenuItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_menuItems.Any(i => i.Id == item.Id)) throw new InvalidOperationException($"Item with ID {item.Id} already exists");
            _menuItems.Add(item);
        }

        public bool RemoveMenuItem(string itemId)
        {
            var item = _menuItems.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _menuItems.Remove(item);
                return true;
            }
            return false;
        }

        public MenuItem GetMenuItem(string itemId)
        {
            return _menuItems.FirstOrDefault(i => i.Id == itemId);
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return new List<MenuItem>(_menuItems.OrderBy(i => i.Category).ThenBy(i => i.Name));
        }

        public List<MenuItem> GetAvailableItems()
        {
            return _menuItems.Where(i => i.IsAvailable)
                           .OrderBy(i => i.Category)
                           .ThenBy(i => i.Name)
                           .ToList();
        }

        public List<MenuItem> GetItemsByCategory(string category)
        {
            return _menuItems.Where(i => i.Category.Equals(category, StringComparison.OrdinalIgnoreCase) && i.IsAvailable)
                           .OrderBy(i => i.Name)
                           .ToList();
        }

        public List<string> GetCategories()
        {
            return _menuItems.Select(i => i.Category)
                           .Distinct()
                           .OrderBy(c => c)
                           .ToList();
        }

        public void SetItemAvailability(string itemId, bool isAvailable)
        {
            var item = GetMenuItem(itemId);
            if (item != null) item.SetAvailability(isAvailable);
        }

        public List<MenuItem> SearchItems(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return GetAvailableItems();
            return _menuItems.Where(i => i.IsAvailable &&
                (i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 i.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 i.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(i => i.Category)
                .ThenBy(i => i.Name)
                .ToList();
        }

        public void Clear()
        {
            _menuItems.Clear();
        }
    }
}