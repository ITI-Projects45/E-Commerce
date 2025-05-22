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

    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PaymentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var payments = unitOfWork.PayementRepo.GetAll().Select(p => new PaymentDTO
            {
                Id = p.Id,
                OrderId = p.OrderId,
                PaymentMethod = p.PaymentMethod,
                PaidAt = p.PaidAt
            }).ToList();

            return Ok(payments);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var payment = unitOfWork.PayementRepo.GetById(id);
            if (payment == null) return NotFound();

            var dto = new PaymentDTO
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                PaidAt = payment.PaidAt
            };

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create(PaymentDTO dto)
        {
            var payment = new Payment
            {
                OrderId = dto.OrderId,
                PaymentMethod = dto.PaymentMethod,
                PaidAt = dto.PaidAt
            };

            unitOfWork.PayementRepo.Create(payment);
            unitOfWork.SaveChangesAsync();

            dto.Id = payment.Id;
            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, dto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, PaymentDTO dto)
        {
            if (id != dto.Id) return BadRequest();

            var payment = unitOfWork.PayementRepo.GetById(id);
            if (payment == null) return NotFound();

            payment.OrderId = dto.OrderId;
            payment.PaymentMethod = dto.PaymentMethod;
            payment.PaidAt = dto.PaidAt;

            unitOfWork.PayementRepo.Update(payment);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var payment = unitOfWork.PayementRepo.GetById(id);
            if (payment == null) return NotFound();
            payment.IsDeleted = true;

            unitOfWork.PayementRepo.Update(payment);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }


    }
}
