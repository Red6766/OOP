using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Models
{
    public class Order
    {
        public string Id { get; }
        public Customer Customer { get; }
        public List<MenuItem> Items { get; }
        public OrderStatus Status { get; protected set; }
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; protected set; }
        public string SpecialInstructions { get; protected set; }
        public OrderType Type { get; }

        public Order(Customer customer, OrderType type = OrderType.Standard)
        {
            Id = Guid.NewGuid().ToString();
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            Items = new List<MenuItem>();
            Status = OrderStatus.Created;
            CreatedAt = DateTime.Now;
            Type = type;
        }

        public virtual void AddItem(MenuItem item)
        {
            if (item?.IsAvailable == true) Items.Add(item);
        }

        public virtual void RemoveItem(MenuItem item)
        {
            Items.Remove(item);
        }

        public virtual void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.Now;
        }

        public virtual int EstimatePreparationTime()
        {
            var baseTime = Items.Any() ? Items.Max(item => item.PreparationTime) : 0;
            return Type == OrderType.Express ? baseTime + 15 : baseTime + 30;
        }
    }

    public enum OrderType
    {
        Standard,
        Express
    }
}