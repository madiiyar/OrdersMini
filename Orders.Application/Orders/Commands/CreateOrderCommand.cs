using FluentValidation;
using MediatR;
using Orders.Application.Abstractions;
using Orders.Domain.Entities;

namespace Orders.Application.Orders.Commands
{
    public record CreateOrderCommand(string CustomerName, decimal TotalAmount) : IRequest<Guid>;

    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
        }
    }

    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrdersRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IOrderHistoryWriter _history;

        public CreateOrderHandler(IOrdersRepository repo, IUnitOfWork uow, IOrderHistoryWriter history)
            => (_repo, _uow, _history) = (repo, uow, history);

        public async Task<Guid> Handle(CreateOrderCommand r, CancellationToken ct)
        {
            var o = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = r.CustomerName,
                TotalAmount = r.TotalAmount,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.New
            };
            await _repo.AddAsync(o, ct);
            await _uow.SaveChangesAsync(ct);

            await _history.WriteAsync(new
            {
                OrderId = o.Id,
                Action = "Created",
                Timestamp = DateTime.UtcNow,
                OldValue = (object?)null,
                NewValue = new { o.CustomerName, o.TotalAmount, o.Status }
            }, ct);

            return o.Id;
        }
    }
}
