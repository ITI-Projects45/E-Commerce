using System.Runtime.CompilerServices;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.Repos.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using E_Commerce.DAL;
using E_Commerce.DB.DTO;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]

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
            var orders = unitOfWork.OrderRepo
                            .GetAll()
                            .Where(o => !o.IsDeleted)
                            .ToList();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = unitOfWork.OrderRepo.GetById(id);
            if (order == null || order.IsDeleted)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            order.OrderDate = DateTime.Now;

            unitOfWork.OrderRepo.Create(order);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order updatedOrder)
        {
            if (id != updatedOrder.Id)
                return BadRequest("Please Your Order Not Found");

            var OldOrder = unitOfWork.OrderRepo.GetById(id);
            if (OldOrder == null || OldOrder.IsDeleted)
                return NotFound();

            OldOrder.total = updatedOrder.total;
            OldOrder.Status = updatedOrder.Status;
            OldOrder.AddresId = updatedOrder.AddresId;
            OldOrder.Payment = updatedOrder.Payment;

            unitOfWork.OrderRepo.Update(OldOrder);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = unitOfWork.OrderRepo.GetById(id);
            if (order == null || order.IsDeleted)
                return NotFound();

            order.IsDeleted = true;
            unitOfWork.OrderRepo.Update(order);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
