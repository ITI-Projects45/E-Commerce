using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CartItemController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IQueryable<CartItem> cartItems = unitOfWork.CartItemRepo.GetAll();

            if (cartItems.Count() == 0)
            {
                ModelState.AddModelError("", "No Items Found");
                return BadRequest(ModelState);
            }
            return Ok(cartItems);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            CartItem cartItem = unitOfWork.CartItemRepo.GetById(id);
            if (cartItem == null)
            {
                ModelState.AddModelError("", "No Item Found");
                return BadRequest(ModelState);
            }
            return Ok(cartItem);
        }

        [HttpPost]
        public IActionResult AddCartItem(CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.CartItemRepo.Create(cartItem);
                    unitOfWork.SaveChangesAsync();
                    return CreatedAtAction("GetById", cartItem.Id, cartItem);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdateCartItem(CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.CartItemRepo.Update(cartItem);
                    unitOfWork.SaveChangesAsync();
                    return CreatedAtAction("GetById", cartItem.Id, cartItem);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCartItem(int id)
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
