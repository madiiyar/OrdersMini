using MediatR;
using Orders.Application.Abstractions;

namespace Orders.Application.Orders.Queries
{
    public record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto?>;

    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IOrdersRepository _repo;
        public GetOrderByIdHandler(IOrdersRepository repo) => _repo = repo;

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken ct)
        {
            var o = await _repo.GetByIdAsync(request.Id, ct);
            return o is null ? null : new OrderDto(o.Id, o.CustomerName, o.TotalAmount, o.CreatedAt, o.Status);
        }
    }
}
