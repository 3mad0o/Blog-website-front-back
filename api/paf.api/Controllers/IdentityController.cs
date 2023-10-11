using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using paf.api.Dtos.Identity_Dtos;
using paf.api.Dtos.User_Dtos;
using paf.api.Interfaces;
using paf.api.validation.user;

namespace paf.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController:ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserService userService;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IMapper mapper;
        private readonly UserCreateValidatior userCreateValidator;
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityController(UserManager<IdentityUser> userManager,
            IUserService userService,
            SignInManager<IdentityUser> signInManager,
            IMapper mapper,
            UserCreateValidatior UserCreateValidator,
            IJwtTokenGenerator jwtTokenGenerator,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.signInManager = signInManager;
            this.mapper = mapper;
            userCreateValidator = UserCreateValidator;
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.roleManager = roleManager;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegister)
        {
            var mappedUser= mapper.Map<UserCreateDto>(userRegister);
            await userCreateValidator.ValidateAndThrowAsync(mappedUser);

            if(!await roleManager.RoleExistsAsync(userRegister.Role))
            {
                await roleManager.CreateAsync(new IdentityRole(userRegister.Role));
            }
            var theUser = new IdentityUser
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                PhoneNumber = userRegister.Phone,
            };
            var result = await userManager.CreateAsync(theUser,userRegister.Password);
            if(result.Succeeded)
            {
                var id= await userService.CreateUser(mappedUser);
                var userFromDb =await userManager.FindByEmailAsync(userRegister.Email);
                await userManager.AddToRoleAsync(userFromDb, userRegister.Role);
                return Ok(new
                {
                    Id=id,
                    Result=result,
                    UserName=theUser.UserName,
                    Email=theUser.Email,
                });
            }
            return BadRequest(result);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            var UserFromDb = await userManager.FindByEmailAsync(userLogin.Email);
            if(UserFromDb == null)
            {
                return BadRequest(new
                {
                    error = "no user found with this email"
                });
            }
            var result =await signInManager.CheckPasswordSignInAsync(UserFromDb, userLogin.Password,false);
            if(result.Succeeded)
            {
                var Role = await userManager.GetRolesAsync(UserFromDb);
                return Ok(new
                {
                    Result = result,
                    Rsername = UserFromDb.UserName,
                    Email = UserFromDb.Email,
                    Toekn = jwtTokenGenerator.GenerateToken(UserFromDb, Role)
                }) ;
            }
            return BadRequest(result);
        }

    }
}
