using System.Runtime.CompilerServices;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.Repos.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using E_Commerce.DAL;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ProductDTO> products = unitOfWork.ProductRepo.GetAll().Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category.Name,
            }).ToList();
            return Ok(products);

        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var product = unitOfWork.ProductRepo.GetById(id);
            if (product == null) {
                return NotFound();
            }
            ProductDTO productDTO = new ProductDTO
            {
                Id = product.Id,
                Brand = product.Brand,
                Category=product.Category.Name,
                Description=product.Description,
                Name=product.Name,
                Price=product.Price,
                Stock=product.Stock
            };
            return Ok(productDTO);
        
        }
        [HttpPost]
        public IActionResult Create(ProductDTO dto)
        {
            var category = unitOfWork.CategoryRepo.GetAll().FirstOrDefault(c => c.Name == dto.Category);
            if (category == null)
            {
                return BadRequest("Invalid category.");
            }

            var p = new Product
            {
                Brand = dto.Brand,
                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = category,
            };

            unitOfWork.ProductRepo.Create(p);
            unitOfWork.SaveChangesAsync();

            dto.Id = p.Id;
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, dto);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductDTO dto)
        {
            if (id != dto.Id) return BadRequest();

            var p = unitOfWork.ProductRepo.GetById(id);
            if (p == null) return NotFound();

            var category = unitOfWork.CategoryRepo.GetAll().FirstOrDefault(c => c.Name == dto.Category);
            if (category == null)
            {
                return BadRequest("Invalid category.");
            }

            p.Name = dto.Name;
            p.Description = dto.Description;
            p.Price = dto.Price;
            p.Stock = dto.Stock;
            p.Brand = dto.Brand;
            p.Category = category;

            unitOfWork.ProductRepo.Update(p);
            unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var p = unitOfWork.ProductRepo.GetById(id);
            if (p == null) return NotFound();
            p.IsDeleted = true;

            unitOfWork.ProductRepo.Update(p);
            unitOfWork.SaveChangesAsync();
            return NoContent();
        }



    }
}
