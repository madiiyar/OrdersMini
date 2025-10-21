using MediatR;
using Orders.Application.Abstractions;

public record DeleteOrderCommand(Guid Id) : IRequest;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrdersRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IOrderHistoryWriter _history;

    public DeleteOrderHandler(IOrdersRepository repo, IUnitOfWork uow, IOrderHistoryWriter history)
        => (_repo, _uow, _history) = (repo, uow, history);

    public async Task Handle(DeleteOrderCommand r, CancellationToken ct)
    {
        var o = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Order not found");
        var old = new { o.CustomerName, o.TotalAmount, o.Status };

        await _repo.RemoveAsync(o, ct);
        await _uow.SaveChangesAsync(ct);

        await _history.WriteAsync(new
        {
            OrderId = o.Id,
            Action = "Deleted",
            Timestamp = DateTime.UtcNow,
            OldValue = old,
            NewValue = (object?)null
        }, ct);
    }
}
