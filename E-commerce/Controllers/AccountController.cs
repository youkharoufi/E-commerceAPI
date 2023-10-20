using E_commerce.Models;
using E_commerce.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _token;
        public AccountController(UserManager<ApplicationUser> userManage,
            SignInManager<ApplicationUser> signInManager, ITokenService token)
        {
            _userManager = userManage;
            _signInManager = signInManager;
            _token = token;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> register(RegisterUser registerUser)
        {
            var newUser = new ApplicationUser
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                AddressLineOne = registerUser.AddressLineOne,
                AddressLineTwo = registerUser.AddressLineTwo,
                City = registerUser.City,
                State = registerUser.State,
                Country = registerUser.Country,
                ZipPostalCode = registerUser.ZipPostalCode,
                DateOfBirth = registerUser.DateOfBirth,
                Firstname = registerUser.Firstname,
                Lastname = registerUser.Lastname,
                Title = registerUser.Title,
                HomePhone = registerUser.HomePhone,
                MobilePhone = registerUser.MobilePhone,
                Token = ""
            };

            var result = await _userManager.CreateAsync(newUser, registerUser.Password);

            if(!result.Succeeded) return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> login(LoginUser loginUser)
        {
            var userFromDb = await _userManager.Users.FirstOrDefaultAsync(usr => usr.Email == loginUser.EmailOrUserName || usr.UserName == loginUser.EmailOrUserName);

            if (userFromDb == null) return NotFound("No such user in database");

            var result = await _signInManager.CheckPasswordSignInAsync(userFromDb, loginUser.Password, false);

            if (!result.Succeeded) return BadRequest("Invalid Credentials");

            userFromDb.Token = await _token.GenerateToken(userFromDb);

            return Ok(userFromDb);
        }
    }
}
