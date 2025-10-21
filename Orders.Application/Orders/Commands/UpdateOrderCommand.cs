using MediatR;
using FluentValidation;
using Orders.Application.Abstractions;
using Orders.Domain.Entities;
using System.Text.Json.Serialization;

public record UpdateOrderCommand(Guid Id, string CustomerName, decimal TotalAmount,
                                  [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderStatus Status) : IRequest;
public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);

        // Add this validation for the status field
        RuleFor(x => x.Status).IsInEnum().WithMessage("Invalid order status");
    }
}

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrdersRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IOrderHistoryWriter _history;

    public UpdateOrderHandler(IOrdersRepository repo, IUnitOfWork uow, IOrderHistoryWriter history)
        => (_repo, _uow, _history) = (repo, uow, history);

    public async Task Handle(UpdateOrderCommand r, CancellationToken ct) 
    {
        var o = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Order not found");
        var old = new { o.CustomerName, o.TotalAmount, o.Status };

        o.CustomerName = r.CustomerName;
        o.TotalAmount = r.TotalAmount;
        o.Status = r.Status;

        await _uow.SaveChangesAsync(ct);

        await _history.WriteAsync(new
        {
            OrderId = o.Id,
            Action = "Updated",
            Timestamp = DateTime.UtcNow,
            OldValue = old,
            NewValue = new { o.CustomerName, o.TotalAmount, o.Status }
        }, ct);
    }
}
