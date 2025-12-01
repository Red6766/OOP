using FoodDelivery.Models;
using FoodDelivery.Services;
using Xunit;

namespace FoodDelivery.Tests
{
    //Тесты для моделей (Models)
    public class MenuItemTests
    {
        [Fact]
        public void Constructor_CreatesValidMenuItem()
        {
            var item = new MenuItem("P001", "Pizza", "Delicious", 12.99m, "Pizza");
            Assert.Equal("P001", item.Id);
            Assert.Equal(12.99m, item.Price);
            Assert.True(item.IsAvailable);
        }

        [Fact]
        public void SetAvailability_ChangesAvailability()
        {
            var item = new MenuItem("P001", "Pizza", "Desc", 10m, "Pizza");
            item.SetAvailability(false);
            Assert.False(item.IsAvailable);
        }
    }

    public class OrderTests
    {
        private readonly Customer _customer = new("C001", "John", "email", "123", "Address");
        private readonly MenuItem _pizza = new("P001", "Pizza", "Desc", 12.99m, "Pizza");

        [Fact]
        public void CreateOrder_StandardType_SetsCorrectProperties()
        {
            var order = new Order(_customer, OrderType.Standard);
            Assert.Equal(OrderType.Standard, order.Type);
            Assert.Equal(OrderStatus.Created, order.Status);
        }

        [Fact]
        public void AddItem_AddsToOrder()
        {
            var order = new Order(_customer, OrderType.Standard);
            order.AddItem(_pizza);
            Assert.Single(order.Items);
        }

        [Fact]
        public void CalculateBasePrice_ReturnsCorrectSum()
        {
            var order = new Order(_customer, OrderType.Standard);
            order.AddItem(_pizza);
            order.AddItem(new MenuItem("D001", "Cola", "Desc", 2.49m, "Drinks"));
            var sum = order.Items.Sum(item => item.Price);
            Assert.Equal(15.48m, sum);
        }

        [Fact]
        public void UpdateStatus_ChangesStatus()
        {
            var order = new Order(_customer, OrderType.Standard);
            order.UpdateStatus(OrderStatus.Preparing);
            Assert.Equal(OrderStatus.Preparing, order.Status);
        }
    }

    //Тесты для сервисов (Services)
    public class MenuServiceTests
    {
        private readonly MenuService _service;

        public MenuServiceTests()
        {
            _service = MenuService.Instance;
            _service.Clear();
        }

        [Fact]
        public void Instance_ReturnsSingleton()
        {
            var instance1 = MenuService.Instance;
            var instance2 = MenuService.Instance;
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void AddMenuItem_AddsToMenu()
        {
            var item = new MenuItem("TEST1", "Pizza", "Desc", 12.99m, "Pizza");
            _service.AddMenuItem(item);
            
            var retrieved = _service.GetMenuItem("TEST1");
            Assert.Equal("Pizza", retrieved.Name);
        }

        [Fact]
        public void GetAvailableItems_ReturnsOnlyAvailable()
        {
            var item = new MenuItem("TEST2", "Pizza", "Desc", 12.99m, "Pizza");
            item.SetAvailability(false);
            _service.AddMenuItem(item);
            
            Assert.Empty(_service.GetAvailableItems());
        }
    }

    public class OrderServiceTests
    {
        private readonly Customer _customer = new("C001", "John", "email", "123", "Address");
        private readonly MenuItem _pizza = new("P001", "Pizza", "Desc", 12.99m, "Pizza");

        [Fact]
        public void CreateAndAddOrder_CreatesOrder()
        {
            var service = new OrderService();
            var order = service.CreateAndAddOrder(_customer, OrderType.Standard);
            Assert.NotNull(order);
            Assert.Equal(OrderStatus.Created, order.Status);
        }

        [Fact]
        public void UpdateOrderStatus_ChangesStatus()
        {
            var service = new OrderService();
            var order = service.CreateAndAddOrder(_customer, OrderType.Standard);
            
            service.UpdateOrderStatus(order.Id, OrderStatus.Preparing);
            var updated = service.GetOrder(order.Id);
            
            Assert.Equal(OrderStatus.Preparing, updated.Status);
        }

        [Fact]
        public void CalculateOrderTotal_CalculatesCorrectly()
        {
            var service = new OrderService();
            var order = service.CreateAndAddOrder(_customer, OrderType.Standard, new[] { _pizza });
            
            var total = service.CalculateOrderTotal(order.Id);
            Assert.Equal(15.98m, total); // 12.99 + 2.99
        }

        [Fact]
        public void AddItemToOrder_AddsItem()
        {
            var service = new OrderService();
            var order = service.CreateAndAddOrder(_customer, OrderType.Standard);
            
            var added = service.AddItemToOrder(order.Id, _pizza);
            Assert.True(added);
        }
    }

    //Тесты для паттернов (Patterns)
    public class PatternTests
    {
        private readonly Customer _customer = new("C001", "John", "email", "123", "Address");

        [Fact]
        public void FactoryMethod_CreatesDifferentOrderTypes()
        {
            OrderFactory standardFactory = new StandardOrderFactory();
            OrderFactory expressFactory = new ExpressOrderFactory();
            
            var standard = standardFactory.CreateOrder(_customer);
            var express = expressFactory.CreateOrder(_customer);
            
            Assert.Equal(OrderType.Standard, standard.Type);
            Assert.Equal(OrderType.Express, express.Type);
        }

        [Fact]
        public void Builder_CreatesComplexOrder()
        {
            var pizza = new MenuItem("P001", "Pizza", "Desc", 12.99m, "Pizza");
            
            var order = new OrderBuilder()
                .WithCustomer(_customer)
                .AsExpress()
                .AddItem(pizza)
                .Build();
            
            Assert.Equal(OrderType.Express, order.Type);
            Assert.Single(order.Items);
        }

        [Fact]
        public void TemplateMethod_CalculatesDifferentPrices()
        {
            var order = new Order(_customer, OrderType.Standard);
            order.AddItem(new MenuItem("P001", "Pizza", "Desc", 10m, "Pizza"));
            
            var standardCalc = new StandardOrderCalculator();
            var expressCalc = new ExpressOrderCalculator();
            
            var standardPrice = standardCalc.CalculateTotal(order);
            var expressPrice = expressCalc.CalculateTotal(order);
            
            Assert.NotEqual(standardPrice, expressPrice);
        }

        [Fact]
        public void Observer_NotifiesOnChanges()
        {
            var notifier = new OrderNotifier();
            var testObserver = new TestObserver();
            notifier.Subscribe(testObserver);
            
            var order = new Order(_customer, OrderType.Standard);
            notifier.NotifyOrderCreated(order);
            
            // Проверяем что наблюдатель был уведомлен
            Assert.True(testObserver.OrderCreatedCalled);
        }

        private class TestObserver : IOrderObserver
        {
            public bool OrderCreatedCalled { get; private set; }
            public bool StatusChangedCalled { get; private set; }
            public bool ItemAddedCalled { get; private set; }

            public void OnOrderCreated(Order order) => OrderCreatedCalled = true;
            public void OnOrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus) => StatusChangedCalled = true;
            public void OnOrderItemAdded(Order order, MenuItem item) => ItemAddedCalled = true;
        }
    }
}