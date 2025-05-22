using System;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce.DB.DTO;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
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
            var responseHelper = new ResponseHelper();
            try
            {
                if (productId <= 0)
                {
                    return BadRequest(responseHelper.BadRequest("Invalid Product ID."));
                }

                var reviews = _unitOfWork.ReviewRepo.GetAll()
                    .Where(e => e.ProductId == productId)
                    .Select(s => new DTOReviews
                    {
                        Comment = s.Comment,
                        Rating = s.Rating,
                        ProductId = s.ProductId
                    })
                    .ToList();

                return Ok(responseHelper.Success(reviews));
            }
            catch (Exception ex)
            {
                return StatusCode(500, responseHelper.ServerError(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DTOReviews dtoReviews)
        {
            var responseHelper = new ResponseHelper();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(responseHelper.WithValidation(ModelState));
                }

                // Optional: Validate Product existence
                var product = _unitOfWork.ProductRepo.GetById(dtoReviews.ProductId ?? 0);
                if (product == null)
                {
                    return NotFound(responseHelper.NotFound("Product not found."));
                }

                var review = new Review
                {
                    Comment = dtoReviews.Comment,
                    Rating = dtoReviews.Rating ?? 0,
                    ProductId = dtoReviews.ProductId ?? 0
                };

                _unitOfWork.ReviewRepo.Create(review);
                await _unitOfWork.SaveChangesAsync();

                return Ok(responseHelper.Created(new DTOReviews
                {
                    Comment = review.Comment,
                    Rating = review.Rating,
                    ProductId = review.ProductId
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, responseHelper.ServerError(ex.Message));
            }
        }
    }
}
