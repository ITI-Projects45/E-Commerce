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
    //[Authorize(Roles = "admin")]
    [AllowAnonymous]
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
            try
            {
                var payments = unitOfWork.PayementRepo.GetAll()
                    .Where(p => !p.IsDeleted)
                    .Select(p => new PaymentDTO
                    {
                        Id = p.Id,
                        OrderId = p.OrderId,
                        PaymentMethod = p.PaymentMethod,
                        PaidAt = p.PaidAt
                    }).ToList();

                return Ok(new ResponseHelper().Success(payments));
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
                var payment = unitOfWork.PayementRepo.GetById(id);
                if (payment == null || payment.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Payment with ID {id} not found"));

                var dto = new PaymentDTO
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    PaymentMethod = payment.PaymentMethod,
                    PaidAt = payment.PaidAt
                };

                return Ok(new ResponseHelper().Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper().WithValidation(ModelState).BadRequest("Invalid payment data"));

                var payment = new Payment
                {
                    OrderId = dto.OrderId,
                    PaymentMethod = dto.PaymentMethod,
                    PaidAt = dto.PaidAt
                };

                unitOfWork.PayementRepo.Create(payment);
                await unitOfWork.SaveChangesAsync();

                dto.Id = payment.Id;

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, new ResponseHelper().Created(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PaymentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper().WithValidation(ModelState).BadRequest("Invalid payment data"));

                if (id != dto.Id)
                    return BadRequest(new ResponseHelper().BadRequest("ID mismatch between URL and body"));

                var payment = unitOfWork.PayementRepo.GetById(id);
                if (payment == null || payment.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Payment with ID {id} not found"));

                payment.OrderId = dto.OrderId;
                payment.PaymentMethod = dto.PaymentMethod;
                payment.PaidAt = dto.PaidAt;

                unitOfWork.PayementRepo.Update(payment);
                await unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Updated(dto));
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
                var payment = unitOfWork.PayementRepo.GetById(id);
                if (payment == null || payment.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Payment with ID {id} not found"));

                payment.IsDeleted = true;
                unitOfWork.PayementRepo.Update(payment);
                await unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Success($"Payment with ID {id} has been deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }
    }
}
