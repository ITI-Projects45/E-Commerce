using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.Repos.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_Commerce.DB.DTO;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "admin")]
    [AllowAnonymous]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var products = unitOfWork.ProductRepo.GetAll()
                    .Where(p => !p.IsDeleted)
                    .Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Brand = p.Brand,
                        Description = p.Description,
                        Price = p.Price,
                        Stock = p.Stock,
                        Category = p.Category.Name,
                    }).ToList();

                return Ok(new ResponseHelper().Success(products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"Error fetching products: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var product = unitOfWork.ProductRepo.GetById(id);
                if (product == null || product.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Product with ID {id} not found"));

                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    Category = product.Category.Name,
                };

                return Ok(new ResponseHelper().Success(productDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"Error fetching product: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper().WithValidation(ModelState).BadRequest("Invalid product data"));

                var category = unitOfWork.CategoryRepo.GetAll().FirstOrDefault(c => c.Name == dto.Category);
                if (category == null)
                    return BadRequest(new ResponseHelper().BadRequest("Invalid category"));
                var product = new Product
                {
                    Name = dto.Name,
                    Brand = dto.Brand,
                    Description = dto.Description,
                    Price = dto.Price,
                    Stock = dto.Stock,
                    Category = category,
                    
                };
                unitOfWork.ProductRepo.Create(product);
                await unitOfWork.SaveChangesAsync();

                List<Image> images = new List<Image>();
                foreach(var img in dto.ImageUrls)
                {
                    unitOfWork.ImageRepo.Create(new Image()
                    {
                        AltText = dto.Name,
                        URL = img,
                        ProductId = product.Id
                    });
                }


                

                await unitOfWork.SaveChangesAsync();

                dto.Id = product.Id;

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, new ResponseHelper().Created(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"Error creating product: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseHelper().WithValidation(ModelState).BadRequest("Invalid product data"));

                if (id != dto.Id)
                    return BadRequest(new ResponseHelper().BadRequest("Product ID mismatch"));

                var product = unitOfWork.ProductRepo.GetById(id);
                if (product == null || product.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Product with ID {id} not found"));

                var category = unitOfWork.CategoryRepo.GetAll().FirstOrDefault(c => c.Name == dto.Category);
                if (category == null)
                    return BadRequest(new ResponseHelper().BadRequest("Invalid category"));

                product.Name = dto.Name;
                product.Brand = dto.Brand;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.Stock = dto.Stock;
                product.Category = category;

                unitOfWork.ProductRepo.Update(product);
                await unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Updated(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"Error updating product: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = unitOfWork.ProductRepo.GetById(id);
                if (product == null || product.IsDeleted)
                    return NotFound(new ResponseHelper().NotFound($"Product with ID {id} not found"));

                product.IsDeleted = true;
                unitOfWork.ProductRepo.Update(product);
                await unitOfWork.SaveChangesAsync();

                return Ok(new ResponseHelper().Success($"Product with ID {id} has been deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseHelper().BadRequest($"Error deleting product: {ex.Message}"));
            }
        }
    }
}
