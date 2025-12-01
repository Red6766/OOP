using System;

namespace FoodDelivery.Models
{
    public class Customer
    {
        public string Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public string Address { get; }
        public DateTime RegistrationDate { get; }

        public Customer(string id, string name, string email, string phone, string address)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            RegistrationDate = DateTime.Now;
        }
    }
}