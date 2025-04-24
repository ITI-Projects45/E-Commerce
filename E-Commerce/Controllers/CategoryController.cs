using System.Runtime.CompilerServices;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.Repos.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using E_Commerce.DAL;
using E_Commerce.DB.DTO;
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

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = unitOfWork.CategoryRepo.GetAll()
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = unitOfWork.CategoryRepo.GetById(id);
            if (category == null)
                return NotFound();

            var dto = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            unitOfWork.CategoryRepo.Create(category);
             unitOfWork.SaveChangesAsync();

            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var category = unitOfWork.CategoryRepo.GetById(id);
            if (category == null)
                return NotFound();

            category.Name = dto.Name;
            category.Description = dto.Description;

            unitOfWork.CategoryRepo.Update(category);
             unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = unitOfWork.CategoryRepo.GetById(id);
            if (category == null)
                return NotFound();

            unitOfWork.CategoryRepo.Delete(id);
             unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
