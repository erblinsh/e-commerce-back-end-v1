﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.UserRepository;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Presistence;
using Life_Ecommerce.TokenService;

namespace Life_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserRepository userRepository;
        public readonly APIDbContext _context;

        public UserController(IUserRepository userRepository, APIDbContext con)
        {
            this.userRepository = userRepository;
            this._context = con;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User u)
        {
             await userRepository.AddUser(u);
            return Ok(u); 

        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> Get()
        {
            var users = await userRepository.GetUsers();
            return Ok(users);
        }
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Put(User user)
        {
            await userRepository.UpdateUser(user);
            return Ok("Updated Successfully");
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] UserLogin Request)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email == Request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(Request.Password, user.Password))
            {
                return Unauthorized("Mejli ose fjalkalimi eshte gabim");
            }

            var roleName = userRepository.GetUserRole(user.RoleId);
            var email = Request.Email;

            var token = TokenService.TokenService.GenerateToken(user.Id, roleName, email);


            return Ok(new
            {
                IsAuthenticated = true,
                Role = roleName,
                Token = token
            });
        }

        public class UserLogin
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpDelete]
        [Route("DeleteUser")]  
        public JsonResult Delete(int id)
        {
            userRepository.DeleteUser(id);
            return new JsonResult("Deleted Successfully");
        }


        [HttpGet]
        [Route("GetUserByID/{Id}")]
        public async Task<IActionResult> GetUserByID(int Id)
        {
            return Ok(await userRepository.GetUserById(Id));
        }

        [HttpGet]
        [Route("GetUsersByRoleId")]
        public async Task<IActionResult> GetUsersByRoleId(int roleId)
        {
            var users = await userRepository.GetUsersByRoleId(roleId);
            return Ok(users);
        }


    }
}