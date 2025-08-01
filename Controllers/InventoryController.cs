using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Application.Features.Inventory.Commands;
using RestaurantManagementSystem.Application.Features.Inventory.Queries;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItemDto>> Create([FromBody] CreateInventoryItemCommand command)
        {
            var item = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItemDto>> Get(int id)
        {
            var item = await _mediator.Send(new GetInventoryItemQuery { Id = id });
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InventoryItemDto>> Update(int id, [FromBody] UpdateInventoryItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Inventory item ID mismatch");
            }

            var updatedItem = await _mediator.Send(command);

            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteInventoryItemCommand { Id = id });

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
