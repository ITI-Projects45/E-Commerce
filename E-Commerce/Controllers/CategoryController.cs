using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet("/Hassan")]
        public IActionResult Index()
        {

            //var Categories = unitOfWork.CategoryRepo.GetAll();

            return Ok(new { x = "Hello World" });
        }
    }
}
