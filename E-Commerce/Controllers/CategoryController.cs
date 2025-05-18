using E_Commerce;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.Repos.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using E_Commerce.DB.DTO;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="admin")]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            try
            {
                var categories = _unitOfWork.CategoryRepo.GetAll(page);

                var categoryDtos = categories
                    .Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToList();


                return Ok(new ResponseHelper().Success(categoryDtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper()
                        .WithValidation(ModelState)
                        .BadRequest("Invalid data"));

                var category = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description
                };

                _unitOfWork.CategoryRepo.Create(category);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return Ok(new ResponseHelper().Created(responseDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var category = _unitOfWork.CategoryRepo.GetById(id);
                if (category == null)
                    return NotFound(new ResponseHelper().NotFound($"Category with ID {id} not found"));

                var dto = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return Ok(new ResponseHelper().Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategoryDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper()
                        .WithValidation(ModelState)
                        .BadRequest("Invalid data"));

                
                var category = _unitOfWork.CategoryRepo.GetById(dto.Id);
                if (category == null)
                    return NotFound(new ResponseHelper().NotFound($"Category with ID {dto.Id} not found"));

                category.Name = dto.Name;
                category.Description = dto.Description;

                _unitOfWork.CategoryRepo.Update(category);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Updated(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = _unitOfWork.CategoryRepo.GetById(id);
                if (category == null)
                    return NotFound(new ResponseHelper().NotFound($"Category with ID {id} not found"));

                _unitOfWork.CategoryRepo.Delete(id);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Success("Category deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"An error occurred: {ex.Message}"));
            }
        }
    }
}