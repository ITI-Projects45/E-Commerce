using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.DB.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "admin")]
    [AllowAnonymous]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ImageController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var images = unitOfWork.ImageRepo
                                       .GetAll()
                                       .Where(img => !img.IsDeleted)
                                       .Select(img => new ImageDTO
                                       {
                                           Id = img.Id,
                                           URL = img.URL,
                                           AltText = img.AltText
                                       })
                                       .ToList();

                return Ok(new ResponseHelper().Success(images));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().ServerError($"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var image = unitOfWork.ImageRepo.GetById(id);
                if (image == null || image.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Image with ID {id} not found"));

                var imageDTO = new ImageDTO
                {
                    Id = image.Id,
                    URL = image.URL,
                    AltText = image.AltText
                };

                return Ok(new ResponseHelper().Success(imageDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().ServerError($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Image image)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper()
                        .WithValidation(ModelState)
                        .BadRequest("Invalid image data"));

                unitOfWork.ImageRepo.Create(image);
                await unitOfWork.SaveChangesAsync();

                var imageDTO = new ImageDTO
                {
                    Id = image.Id,
                    URL = image.URL,
                    AltText = image.AltText
                };

                return Ok(new ResponseHelper().Created(imageDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().ServerError($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Image updatedImage)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper()
                        .WithValidation(ModelState)
                        .BadRequest("Invalid update data"));

                if (id != updatedImage.Id)
                    return BadRequest(new ResponseHelper().BadRequest("Mismatched ID"));

                var oldImage = unitOfWork.ImageRepo.GetById(id);
                if (oldImage == null || oldImage.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Image with ID {id} not found"));

                oldImage.URL = updatedImage.URL;
                oldImage.AltText = updatedImage.AltText;
                oldImage.Product = updatedImage.Product;

                unitOfWork.ImageRepo.Update(oldImage);
                await unitOfWork.SaveChangesAsync();

                var dto = new ImageDTO
                {
                    Id = oldImage.Id,
                    URL = oldImage.URL,
                    AltText = oldImage.AltText
                };

                return Ok(new ResponseHelper().Updated(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().ServerError($"An error occurred: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var image = unitOfWork.ImageRepo.GetById(id);
                if (image == null || image.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Image with ID {id} not found"));

                image.IsDeleted = true;
                unitOfWork.ImageRepo.Update(image);
                await unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Success("Image deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().ServerError($"An error occurred: {ex.Message}"));
            }
        }
    }
}
