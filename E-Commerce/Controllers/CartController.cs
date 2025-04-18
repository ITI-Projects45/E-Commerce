using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IQueryable<CartItem> addresses = unitOfWork.CartItemRepo.GetAll();

            if (addresses.Count() == 0)
            {
                ModelState.AddModelError("", "No Address Found");
                return BadRequest(ModelState);
            }
            return Ok(addresses);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            CartItem address = unitOfWork.CartItemRepo.GetById(id);
            if (address == null)
            {
                ModelState.AddModelError("", "No Address Found");
                return BadRequest(ModelState);
            }
            return Ok(address);
        }

        [HttpPost]
        public IActionResult AddAddress(CartItem address)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.CartItemRepo.Create(address);
                    unitOfWork.SaveChangesAsync();
                    return CreatedAtAction("GetById", address.Id, address);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdateAddress(CartItem address)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.CartItemRepo.Update(address);
                    unitOfWork.SaveChangesAsync();
                    return CreatedAtAction("GetById", address.Id, address);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteAddress(int id)
        {
            if (unitOfWork.CartItemRepo.GetById(id) != null)
            {
                try
                {
                    unitOfWork.CartItemRepo.Delete(id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
