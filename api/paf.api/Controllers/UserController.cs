using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using paf.api.Dtos.User_Dtos;
using paf.api.Interfaces;
using paf.api.Models;

namespace paf.api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateUser(UserCreateDto user)
        {
           var Id= await userService.CreateUser(user);
            return Ok(Id);
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUser(UserDeleteDto userDeleteDto)
        {
           var entity =await userService.GetUserById(userDeleteDto.Id);
            userService.DeleteUser(userDeleteDto);
            return Ok(new {
            isDeleted ="yes"
            });
        }
        [HttpGet]
        [Route("Get/Email/{Email}")]
        public async Task<IActionResult> GetUser(string Email)
        {
           var theUser= await userService.GetUser(Email);
            return Ok(theUser);
        }
        
        [HttpGet]
        [Route("Get/Id/{Id}")]
        public async Task<IActionResult> GetUserById(int Id)
        {
            var theUser = await userService.GetUserById(Id);
            return Ok(theUser);
        }
        
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto user)
        {
             userService.UpdateUser(user);
            return Ok(user.Id);
        }
        [HttpGet]
        [Route("Count")]
        public async Task<IActionResult> CountUsers()
        {
            var number =await userService.NumberOfUsers();
            return Ok(number);
        }
    }
}
