using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.DB.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var orders = unitOfWork.OrderRepo
                    .GetAll()
                    .Where(o => !o.IsDeleted)
                    .ToList();

                return Ok(new ResponseHelper().Success(orders));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var order = unitOfWork.OrderRepo.GetById(id);
                if (order == null || order.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Order with ID {id} not found"));

                return Ok(new ResponseHelper().Success(order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper().WithValidation(ModelState).BadRequest("Invalid order data"));

                order.OrderDate = DateTime.Now;
                unitOfWork.OrderRepo.Create(order);
                await unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Created(order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order updatedOrder)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper().WithValidation(ModelState).BadRequest("Invalid order data"));

                if (id != updatedOrder.Id)
                    return BadRequest(new ResponseHelper().BadRequest("Order ID mismatch"));

                var existingOrder = unitOfWork.OrderRepo.GetById(id);
                if (existingOrder == null || existingOrder.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Order with ID {id} not found"));

                existingOrder.total = updatedOrder.total;
                existingOrder.Status = updatedOrder.Status;
                existingOrder.AddresId = updatedOrder.AddresId;
                existingOrder.Payment = updatedOrder.Payment;

                unitOfWork.OrderRepo.Update(existingOrder);
                await unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Updated(existingOrder));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = unitOfWork.OrderRepo.GetById(id);
                if (order == null || order.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Order with ID {id} not found"));

                order.IsDeleted = true;
                unitOfWork.OrderRepo.Update(order);
                await unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Success("Order deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }
    }
}
