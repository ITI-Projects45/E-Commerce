using System.Data;
using System.Diagnostics.Contracts;
using E_Commerce.DB.DTO.UsersDTO;
using E_Commerce.DB.Models;
using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManger;
        private readonly RoleManager<IdentityRole> userRole;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration config;



        public UsersController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManger,
            RoleManager<IdentityRole> userRole,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config)
        {
            this.unitOfWork = unitOfWork;
            this.userManger = userManger;
            this.userRole = userRole;
            this.signInManager = signInManager;
            this.config = config;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(UserDTO userDTO)
        {
            ResponseHelper responseHelper = new ResponseHelper();
            if (!ModelState.IsValid)
            {
                return BadRequest(responseHelper.BadRequest().WithValidation(ModelState));
            }

            if (await userIsExist(userDTO.Email))
            {
                return BadRequest(responseHelper.BadRequest("User Already Exist"));
            }

            if (!await roleIsExist(userDTO.RoleName))
            {
                return NotFound(responseHelper.NotFound("Role Dose not Exist"));
            }

            ApplicationUser createUser = new ApplicationUser()
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
            };
            await userManger.CreateAsync(createUser, userDTO.Password);

            return Ok(responseHelper.Created(createUser).WithMassage("User Created Successfully"));
        }

        [HttpGet("getAll/{page:int}")]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            if (page < 1) page = 1;

            ResponseHelper responseHelper = new ResponseHelper();
            List<ApplicationUser> users = await userManger.Users.Skip((page - 1) * 10).Take(10).ToListAsync();

            List<UserDataDTO> allUsers = new List<UserDataDTO>();
            foreach (ApplicationUser user in users)
            {
                List<string> rols = (List<string>)await userManger.GetRolesAsync(user);

                allUsers.Add(new UserDataDTO
                {

                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Imageurl = user.imageurl,
                    PhoneNumber = user.PhoneNumber,
                    Roles = rols

                });

            }


            return Ok(responseHelper.Success(allUsers));
        }





        [NonAction]
        public async Task<bool> userIsExist(string userEmail)
        {
            ApplicationUser isEsist = await userManger.FindByEmailAsync(userEmail);
            return isEsist != null;
        }

        [NonAction]
        public async Task<bool> roleIsExist(string role)
        {
            IdentityRole isEsist = await userRole.FindByNameAsync(role);
            return isEsist != null;
        }

    }
}
