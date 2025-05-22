using System.Runtime.CompilerServices;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.Repos.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using E_Commerce.DAL;
using Microsoft.AspNetCore.Authorization;
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
            var items = unitOfWork.OrderItemRepo.GetAll().Select(oi => new OrderItemDTO
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                OrderId = oi.OrderId,
                Quantity = oi.Quantity,
                PriceAtPurchase = oi.PriceAtPurchase
            }).ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = unitOfWork.OrderItemRepo.GetById(id);
            if (item == null) return NotFound();

            var dto = new OrderItemDTO
            {
                Id = item.Id,
                ProductId = item.ProductId,
                OrderId = item.OrderId,
                Quantity = item.Quantity,
                PriceAtPurchase = item.PriceAtPurchase
            };

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create(OrderItemDTO dto)
        {
            var item = new OrderItem
            {
                ProductId = dto.ProductId,
                OrderId = dto.OrderId,
                Quantity = dto.Quantity,
                PriceAtPurchase = dto.PriceAtPurchase
            };

            unitOfWork.OrderItemRepo.Create(item);
            unitOfWork.SaveChangesAsync();

            dto.Id = item.Id;
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, dto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, OrderItemDTO dto)
        {
            if (id != dto.Id) return BadRequest();

            var item = unitOfWork.OrderItemRepo.GetById(id);
            if (item == null) return NotFound();

            item.ProductId = dto.ProductId;
            item.OrderId = dto.OrderId;
            item.Quantity = dto.Quantity;
            item.PriceAtPurchase = dto.PriceAtPurchase;

            unitOfWork.OrderItemRepo.Update(item);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}