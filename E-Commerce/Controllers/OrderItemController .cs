using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.Repos.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_Commerce.DB.DTO;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class OrderItemController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderItemController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var items = unitOfWork.OrderItemRepo.GetAll().Select(oi => new OrderItemDTO
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    OrderId = oi.OrderId,
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase
                }).ToList();

                return Ok(new ResponseHelper().Success(items));
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
                var item = unitOfWork.OrderItemRepo.GetById(id);
                if (item == null)
                    return NotFound(new ResponseHelper().NotFound($"Order item with ID {id} not found"));

                var dto = new OrderItemDTO
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    OrderId = item.OrderId,
                    Quantity = item.Quantity,
                    PriceAtPurchase = item.PriceAtPurchase
                };

                return Ok(new ResponseHelper().Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderItemDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper().WithValidation(ModelState).BadRequest("Invalid order item data"));

                var item = new OrderItem
                {
                    ProductId = dto.ProductId,
                    OrderId = dto.OrderId,
                    Quantity = dto.Quantity,
                    PriceAtPurchase = dto.PriceAtPurchase
                };

                unitOfWork.OrderItemRepo.Create(item);
                await unitOfWork.SaveChangesAsync();

                dto.Id = item.Id;
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, new ResponseHelper().Created(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderItemDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper().WithValidation(ModelState).BadRequest("Invalid order item data"));

                if (id != dto.Id)
                    return BadRequest(new ResponseHelper().BadRequest("ID mismatch between URL and body"));

                var item = unitOfWork.OrderItemRepo.GetById(id);
                if (item == null)
                    return NotFound(new ResponseHelper().NotFound($"Order item with ID {id} not found"));

                item.ProductId = dto.ProductId;
                item.OrderId = dto.OrderId;
                item.Quantity = dto.Quantity;
                item.PriceAtPurchase = dto.PriceAtPurchase;

                unitOfWork.OrderItemRepo.Update(item);
                await unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Updated(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }
    }
}
