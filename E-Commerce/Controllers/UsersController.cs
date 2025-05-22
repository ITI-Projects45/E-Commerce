using System.Data;
using E_Commerce.DB.DTO.UsersDTO;
using E_Commerce.DB.Models;
using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public UsersController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] UserDTO userDTO)
        {
            var response = new ResponseHelper();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(response.BadRequest().WithValidation(ModelState));

                if (await UserExists(userDTO.Email))
                    return BadRequest(response.BadRequest("User already exists"));

                if (!await RoleExists(userDTO.RoleName))
                    return NotFound(response.NotFound("Role does not exist"));

                var newUser = new ApplicationUser
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                };

                var result = await _userManager.CreateAsync(newUser, userDTO.Password);

                if (!result.Succeeded)
                    return BadRequest(response.BadRequest("Failed to create user").WithIdentityErrors(result.Errors));

                await _userManager.AddToRoleAsync(newUser, userDTO.RoleName);

                return Ok(response.Created(newUser).WithMassage("User created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, response.ServerError(ex.Message));
            }
        }

        [HttpGet("getAll/{page:int}")]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            var response = new ResponseHelper();
            try
            {
                if (page < 1) page = 1;

                var users = await _userManager.Users
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToListAsync();

                var userList = new List<UserDataDTO>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    userList.Add(new UserDataDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Imageurl = user.imageurl,
                        PhoneNumber = user.PhoneNumber,
                        Roles = roles.ToList()
                    });
                }

                return Ok(response.Success(userList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, response.ServerError(ex.Message));
            }
        }

        #region Helper Methods

        [NonAction]
        private async Task<bool> UserExists(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            return existingUser != null;
        }

        [NonAction]
        private async Task<bool> RoleExists(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role != null;
        }

        #endregion
    }
}
