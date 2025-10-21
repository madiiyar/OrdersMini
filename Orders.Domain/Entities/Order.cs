using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        New,
        Paid,
        Shipped,
        Cancelled
    }
}
