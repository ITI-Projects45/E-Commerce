using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DB.DTO.UsersDTO;
using E_Commerce.DB.Models;
using E_Commerce.Repos.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManger;
        private readonly RoleManager<IdentityRole> userRole;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration config;



        public AuthController(
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLogin userData)
        {

            ResponseHelper responseHelper = new ResponseHelper();

            if (!ModelState.IsValid)
            {
                return BadRequest(responseHelper.WithValidation(ModelState));
            }

            ApplicationUser login = await userManger.FindByEmailAsync(userData.Email);

            if (login == null)
            {
                return BadRequest(responseHelper.NotFound("Email Dosn't Exsist"));
            }


            var result = await userManger.CheckPasswordAsync(login, userData.Password);
            if (!result)
            {
                return BadRequest(responseHelper.Unauthorized("Invalid Credintional"));
            }

            //var result = await signInManager.PasswordSignInAsync(login.UserName, userData.Password, false, false);

            //if (!result.Succeeded)
            //{
            //    return BadRequest(responseHelper.BadRequest());
            //}





            var obj = await GenerateToken(login);
            return Ok(responseHelper.Success().WithData(obj));
        }


        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody]RegisterAuthDTO registerUser)
        {
            ResponseHelper responseHelper = new ResponseHelper();

            if (!ModelState.IsValid)
            {
                return BadRequest(responseHelper.WithValidation(ModelState));
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,

            };
            IdentityResult res = await userManger.CreateAsync(applicationUser, registerUser.Password);

            if (!res.Succeeded)
            {
                return BadRequest(res.Errors);
            }

            // create tojken 
            var obj = await GenerateToken(applicationUser);


            return Ok(responseHelper.Success().WithData(obj));
        }


        [NonAction]
        public async Task<object> GenerateToken(ApplicationUser applicationUser)
        {

            string jti = Guid.NewGuid().ToString();
            var uerRole = await userManger.GetRolesAsync(applicationUser);
            List<Claim> claim = new List<Claim>();
            claim.Add(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
            claim.Add(new Claim(ClaimTypes.Name, applicationUser.UserName));
            claim.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));//optional

            if (uerRole != null)
            {
                foreach (var role in uerRole)
                {
                    claim.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            SymmetricSecurityKey signinKey =
                           new(Encoding.UTF8.GetBytes(config["JWT:Key"]));//hjhjhjhjh

            SigningCredentials signingCredentials =
                new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken myToken = new JwtSecurityToken(
                issuer: config["JWT:Iss"],
                audience: config["JWT:Aud"],
                expires: DateTime.Now.AddHours(1),
                claims: claim,
                signingCredentials: signingCredentials
                );

            return new
            {
                expired = DateTime.Now.AddHours(1),
                token = new JwtSecurityTokenHandler().WriteToken(myToken)
            }; ;
        }


    }
}
