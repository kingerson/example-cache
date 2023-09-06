using System;

namespace MS.IConstruye.Domain.Aggregates
{
    public class Order
    {
        public Order(
            string name,
            string email,
            string address,
            int productId,
            int quantity,
            DateTime registerTimeStamp
            )
        {
            Name = name;
            Email = email;
            Address = address;
            ProductId = productId;
            Quantity = quantity;   
            RegisterTimeStamp = registerTimeStamp;
        }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public DateTime RegisterTimeStamp { get; private set; }
    }
}
