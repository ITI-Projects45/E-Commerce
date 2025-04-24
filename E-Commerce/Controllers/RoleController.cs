using E_Commerce.DB.DTO.UsersDTO;
using E_Commerce.DB.Models;
using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> userRole;



        public RoleController(
            RoleManager<IdentityRole> userRole)
        {
            this.userRole = userRole;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody]RolsDTO role)
        {
            ResponseHelper responseHelper = new ResponseHelper();
            IdentityRole identityRole = new IdentityRole()
            {
                Name = role.RoleName
            };

            IdentityRole isExist = await userRole.FindByNameAsync(identityRole.Name);

            if (isExist != null) {
                return BadRequest(responseHelper.BadRequest("Role Already Esist"));
            }

            await userRole.CreateAsync(identityRole);


            return Ok(responseHelper.Created(identityRole));
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            ResponseHelper responseHelper = new ResponseHelper();
            List<IdentityRole> rolesList = await userRole.Roles.ToListAsync();
            return Ok(responseHelper.Success(rolesList));
        }



    }
}
