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
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin")]
    [AllowAnonymous]
    public class VideoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VideoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new ResponseHelper();
            try
            {
                var videos = _unitOfWork.VideoRepo
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

                return Ok(response.Success(videos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, response.ServerError(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var response = new ResponseHelper();
            try
            {
                var video = _unitOfWork.VideoRepo.GetById(id);
                if (video == null || video.IsDeleted)
                    return NotFound(response.NotFound("Video not found"));

                var videoDTO = new VideoDTO
                {
                    Id = video.Id,
                    URL = video.URL,
                    AltText = video.AltText,
                    ProductId = video.Product.Id
                };

                return Ok(response.Success(videoDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, response.ServerError(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VideoDTO videoDTO)
        {
            var response = new ResponseHelper();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(response.WithValidation(ModelState));

                var product = _unitOfWork.ProductRepo.GetById(videoDTO.ProductId);
                if (product == null)
                    return NotFound(response.NotFound("Associated product not found"));

                var video = new Video
                {
                    URL = videoDTO.URL,
                    AltText = videoDTO.AltText,
                    Product = product
                };

                _unitOfWork.VideoRepo.Create(video);
                await _unitOfWork.SaveChangesAsync();

                return Ok(response.Created(video));
            }
            catch (Exception ex)
            {
                return StatusCode(500, response.ServerError(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VideoDTO updatedVideoDTO)
        {
            var response = new ResponseHelper();
            try
            {
                if (id != updatedVideoDTO.Id)
                    return BadRequest(response.BadRequest("Mismatched video ID"));

                if (!ModelState.IsValid)
                    return BadRequest(response.WithValidation(ModelState));

                var existingVideo = _unitOfWork.VideoRepo.GetById(id);
                if (existingVideo == null || existingVideo.IsDeleted)
                    return NotFound(response.NotFound("Video not found"));

                var product = _unitOfWork.ProductRepo.GetById(updatedVideoDTO.ProductId);
                if (product == null)
                    return NotFound(response.NotFound("Associated product not found"));

                existingVideo.URL = updatedVideoDTO.URL;
                existingVideo.AltText = updatedVideoDTO.AltText;
                existingVideo.Product = product;

                _unitOfWork.VideoRepo.Update(existingVideo);
                await _unitOfWork.SaveChangesAsync();

                return Ok(response.Updated(existingVideo));
            }
            catch (Exception ex)
            {
                return StatusCode(500, response.ServerError(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new ResponseHelper();
            try
            {
                var video = _unitOfWork.VideoRepo.GetById(id);
                if (video == null || video.IsDeleted)
                    return NotFound(response.NotFound("Video not found"));

                video.IsDeleted = true;
                _unitOfWork.VideoRepo.Update(video);
                await _unitOfWork.SaveChangesAsync();

                return Ok(response.WithStatus(true).WithMassage($"Video with ID {id} has been deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, response.ServerError(ex.Message));
            }
        }
    }
}
