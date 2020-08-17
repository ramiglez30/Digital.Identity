using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digital.Identity.Admin.Data;
using Digital.Identity.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Digital.Identity.Admin.Controllers
{
    public class UsersController : AdminControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<ApplicationUser> Get()
        {
            var users = _userManager.Users.ToList();
            return users;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ApplicationUser> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] CreateUserInput input)
        {
            var user = new ApplicationUser { UserName = input.UserName, Email = input.Email };
            _userManager.CreateAsync(user, input.Password);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            
        }
    }
}
