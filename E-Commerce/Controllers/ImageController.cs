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

            return Ok(images);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var image = unitOfWork.ImageRepo.GetById(id);
            if (image == null || image.IsDeleted)
                return NotFound();

            var imageDTO = new ImageDTO
            {
                Id = image.Id,
                URL = image.URL,
                AltText = image.AltText
            };

            return Ok(imageDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Image image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            unitOfWork.ImageRepo.Create(image);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Image updatedImage)
        {
            if (id != updatedImage.Id)
                return BadRequest("Mismatched ID");

            var oldImage = unitOfWork.ImageRepo.GetById(id);
            if (oldImage == null || oldImage.IsDeleted)
                return NotFound();

            oldImage.URL = updatedImage.URL;
            oldImage.AltText = updatedImage.AltText;
            oldImage.Product = updatedImage.Product;

            unitOfWork.ImageRepo.Update(oldImage);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var image = unitOfWork.ImageRepo.GetById(id);
            if (image == null || image.IsDeleted)
                return NotFound();

            image.IsDeleted = true;
            unitOfWork.ImageRepo.Update(image);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}