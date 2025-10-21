using Orders.Domain.Entities;

namespace Orders.Application.Orders
{
    public record OrderDto(Guid Id, string CustomerName, decimal TotalAmount, DateTime CreatedAt, OrderStatus Status);
}
