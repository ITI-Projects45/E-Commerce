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


    public class VideoController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public VideoController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var videos = unitOfWork.VideoRepo
                                   .GetAll()
                                   .Where(v => !v.IsDeleted)
                                   .Select(v => new VideoDTO
                                   {
                                       Id = v.Id,
                                       URL = v.URL,
                                       AltText = v.AltText,
                                       ProductId = v.Product.Id
                                   })
                                   .ToList();

            return Ok(videos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var video = unitOfWork.VideoRepo.GetById(id);
            if (video == null || video.IsDeleted)
                return NotFound();

            var videoDTO = new VideoDTO
            {
                Id = video.Id,
                URL = video.URL,
                AltText = video.AltText,
                ProductId = video.Product.Id
            };

            return Ok(videoDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VideoDTO videoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var video = new Video
            {
                URL = videoDTO.URL,
                AltText = videoDTO.AltText,
                Product = unitOfWork.ProductRepo.GetById(videoDTO.ProductId)
            };

            unitOfWork.VideoRepo.Create(video);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VideoDTO updatedVideoDTO)
        {
            if (id != updatedVideoDTO.Id)
                return BadRequest("Mismatched ID");

            var oldVideo = unitOfWork.VideoRepo.GetById(id);
            if (oldVideo == null || oldVideo.IsDeleted)
                return NotFound();

            oldVideo.URL = updatedVideoDTO.URL;
            oldVideo.AltText = updatedVideoDTO.AltText;
            oldVideo.Product = unitOfWork.ProductRepo.GetById(updatedVideoDTO.ProductId);

            unitOfWork.VideoRepo.Update(oldVideo);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var video = unitOfWork.VideoRepo.GetById(id);
            if (video == null || video.IsDeleted)
                return NotFound();

            video.IsDeleted = true;
            unitOfWork.VideoRepo.Update(video);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
