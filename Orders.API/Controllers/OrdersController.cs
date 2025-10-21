using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Orders;
using Orders.Application.Orders.Commands;
using Orders.Application.Orders.Queries;

namespace Orders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public Task<List<OrderDto>> GetAll(CancellationToken ct) =>
        _mediator.Send(new GetOrdersQuery(), ct);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _mediator.Send(new GetOrderByIdQuery(id), ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateOrderCommand cmd, CancellationToken ct)
    {
        var id = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateOrderCommand body, CancellationToken ct)
    {
        if (id != body.Id)
            return BadRequest("Id mismatch");

        try
        {
            await _mediator.Send(body, ct);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
    {
        try { await _mediator.Send(new DeleteOrderCommand(id), ct); return NoContent(); }
        catch (KeyNotFoundException) { return NotFound(); }
    }
}
