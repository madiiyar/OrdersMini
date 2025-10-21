using Microsoft.EntityFrameworkCore;
using Orders.Application.Abstractions;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Persistence.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _db;
        public OrdersRepository(AppDbContext db) => _db = db;

        public Task<List<Order>> GetAllAsync(CancellationToken ct) =>
            _db.Orders.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync(ct);

        public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct) =>
            _db.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task AddAsync(Order order, CancellationToken ct) =>
            await _db.Orders.AddAsync(order, ct);

        public Task RemoveAsync(Order order, CancellationToken ct)
        {
            _db.Orders.Remove(order);
            return Task.CompletedTask;
        }
    }
}
