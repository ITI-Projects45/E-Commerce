using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using E_Commerce.DB.DTO.UsersDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin")]
    [AllowAnonymous]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] RolsDTO role)
        {
            var responseHelper = new ResponseHelper();

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(responseHelper.WithValidation(ModelState));
                }

                if (string.IsNullOrWhiteSpace(role.RoleName))
                {
                    return BadRequest(responseHelper.BadRequest("Role name is required."));
                }

                var existingRole = await _roleManager.FindByNameAsync(role.RoleName);
                if (existingRole != null)
                {
                    return BadRequest(responseHelper.BadRequest("Role already exists."));
                }

                var newRole = new IdentityRole { Name = role.RoleName };
                var result = await _roleManager.CreateAsync(newRole);

                if (!result.Succeeded)
                {
                    return BadRequest(responseHelper.WithIdentityErrors(result.Errors));
                }

                return Ok(responseHelper.Created(newRole));
            }
            catch (Exception ex)
            {
                return StatusCode(500, responseHelper.ServerError(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var responseHelper = new ResponseHelper();

            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                return Ok(responseHelper.Success(roles));
            }
            catch (Exception ex)
            {
                return StatusCode(500, responseHelper.ServerError(ex.Message));
            }
        }
    }
}
