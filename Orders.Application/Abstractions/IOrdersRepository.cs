using Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Abstractions
{
    public interface IOrdersRepository
    {
        Task<List<Order>> GetAllAsync(CancellationToken ct);
        Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(Order order, CancellationToken ct);
        Task RemoveAsync(Order order, CancellationToken ct);
    }
}
