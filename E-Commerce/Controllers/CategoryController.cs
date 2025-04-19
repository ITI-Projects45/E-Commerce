using E_Commerce.DB.DTO;
using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpPost("/Hassan")]
        public IActionResult Index([FromBody] DTOReviews dTOReviews)
        {
            if (ModelState.IsValid) { 
               return Ok(dTOReviews);
            }
            else
            {
                return BadRequest(ModelState);
            }
                var Categories = unitOfWork.CategoryRepo.GetAll();

            return Ok( Categories );
        }
        
        
    
    }
}
