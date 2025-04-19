using System.Linq;
using E_Commerce.DB.DTO;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    //[ApiController]
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewController(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public IActionResult Index(int productId)
        {
            IQueryable<DTOReviews> revs = _unitOfWork.ReviewRepo.GetAll().Where(e=>e.ProductId == productId).Select(s=>new DTOReviews
            {
                Comment = s.Comment,
                Rating = s.Rating,
                ProductId = s.ProductId
            }) ;
            ResponseHelper responseHelper = new ResponseHelper();
            
            return Ok(responseHelper.Success(revs));
        }
        
        [HttpPost]
        public  IActionResult Create([FromBody]DTOReviews dTOReviews)
        {
            ResponseHelper responseHelper = new ResponseHelper();
            if (!ModelState.IsValid)
            {
                return BadRequest(responseHelper.WithValidation(ModelState));
            }

            Review review = new Review()
            {
                Comment = dTOReviews.Comment,
                Rating =(int) dTOReviews.Rating,
                ProductId = (int)dTOReviews.ProductId
            };
            _unitOfWork.ReviewRepo.Create(review);
             _unitOfWork.SaveChangesAsync();
            return Ok(responseHelper.Success(review));
        }


    }
}
