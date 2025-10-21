using MediatR;
using Orders.Application.Abstractions;

namespace Orders.Application.Orders.Queries
{
    public record GetOrdersQuery() : IRequest<List<OrderDto>>;

    public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
    {
        private readonly IOrdersRepository _repo;
        public GetOrdersHandler(IOrdersRepository repo) => _repo = repo;

        public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken ct) =>
            (await _repo.GetAllAsync(ct))
                .Select(o => new OrderDto(o.Id, o.CustomerName, o.TotalAmount, o.CreatedAt, o.Status))
                .ToList();
    }
}
