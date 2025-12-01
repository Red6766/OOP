using System;
using System.Collections.Generic;
using FoodDelivery.Models;

namespace FoodDelivery.Services
{
    public class OrderBuilder
    {
        private Customer _customer;
        private OrderType _type = OrderType.Standard;
        private readonly List<MenuItem> _items = new();
        private string _specialInstructions;

        public OrderBuilder WithCustomer(Customer customer)
        {
            _customer = customer ?? throw new ArgumentNullException(nameof(customer));
            return this;
        }

        public OrderBuilder AsExpress()
        {
            _type = OrderType.Express;
            return this;
        }

        public OrderBuilder AddItem(MenuItem item)
        {
            if (item?.IsAvailable == true) _items.Add(item);
            return this;
        }

        public OrderBuilder AddItems(IEnumerable<MenuItem> items)
        {
            foreach (var item in items) AddItem(item);
            return this;
        }

        public OrderBuilder WithSpecialInstructions(string instructions)
        {
            _specialInstructions = instructions;
            return this;
        }

        public Order Build()
        {
            if (_customer == null) throw new InvalidOperationException("Customer is required");
            var order = new Order(_customer, _type);
            foreach (var item in _items) order.AddItem(item);
            return order;
        }
    }
}