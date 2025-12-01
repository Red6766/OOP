using System;
using System.Collections.Generic;

namespace FoodDelivery.Models
{
    public class MenuItem
    {
        public string Id { get; } 
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string Category { get; private set; }
        public bool IsAvailable { get; private set; } = true;
        public int PreparationTime { get; private set; }

        public MenuItem(string id, string name, string description, decimal price, string category, int preparationTime = 15)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Price = price >= 0 ? price : throw new ArgumentException("Price cannot be negative");
            Category = category;
            PreparationTime = preparationTime > 0 ? preparationTime : 15;
        }

        public void SetAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;
        }
    }
}