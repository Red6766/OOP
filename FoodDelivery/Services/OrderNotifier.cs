using System.Collections.Generic;
using FoodDelivery.Models;

namespace FoodDelivery.Services
{
    public interface IOrderObserver
    {
        void OnOrderCreated(Order order);
        void OnOrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus);
        void OnOrderItemAdded(Order order, MenuItem item);
    }

    public class OrderNotifier
    {
        private readonly List<IOrderObserver> _observers = new();

        public void Subscribe(IOrderObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Unsubscribe(IOrderObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyOrderCreated(Order order)
        {
            foreach (var observer in _observers)
                observer.OnOrderCreated(order);
        }

        public void NotifyStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
        {
            foreach (var observer in _observers)
                observer.OnOrderStatusChanged(order, oldStatus, newStatus);
        }

        public void NotifyItemAdded(Order order, MenuItem item)
        {
            foreach (var observer in _observers)
                observer.OnOrderItemAdded(order, item);
        }
    }

    public class OrderLogger : IOrderObserver
    {
        private readonly List<string> _logs = new();

        public void OnOrderCreated(Order order)
        {
            _logs.Add($"Order created: {order.Id} for {order.Customer.Name}");
        }

        public void OnOrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
        {
            _logs.Add($"Order {order.Id}: {oldStatus} -> {newStatus}");
        }

        public void OnOrderItemAdded(Order order, MenuItem item)
        {
            _logs.Add($"Item added to order {order.Id}: {item.Name}");
        }

        public List<string> GetLogs() => new(_logs);
    }
}