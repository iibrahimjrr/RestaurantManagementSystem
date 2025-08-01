using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Application.Features.POS.Commands;
using RestaurantManagementSystem.Application.Features.POS.Queries;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class POSController : ControllerBase
    {
        private readonly IMediator _mediator;

        public POSController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderCommand command)
        {
            var order = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Get(int id)
        {
            var order = await _mediator.Send(new GetOrderQuery { Id = id });
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> Update(int id, [FromBody] UpdateOrderCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Order ID mismatch");
            }

            var updatedOrder = await _mediator.Send(command);

            if (updatedOrder == null)
            {
                return NotFound();
            }

            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteOrderCommand { Id = id });

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
