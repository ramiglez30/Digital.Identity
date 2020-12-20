using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Digital.Identity.Admin.Models;
using Digital.Identity.Admin.Models.Api;
using Digital.Identity.Admin.Models.EF;
using Digital.Identity.Admin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Digital.Identity.Admin.Controllers
{
#nullable enable
    public class UsersController : AdminControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService; 

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PagedList? pagination = null)
        {
            _logger.LogInformation($"Get users with page: {pagination?.PageNumber}, and page total: {pagination?.PageTotal}");
            var users = await _userService.GetUsersAsync(pagination);

            _logger.LogInformation($"Returning {users.Count} users.");
            return StatusCode(StatusCodes.Status200OK, users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            _logger.LogInformation($"Get user with id: {id}"); 
            var user = await _userService.GetUserAsync(id);

            if(user == null)
            {
                _logger.LogInformation($"User with id: {id} does not exist.");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            _logger.LogInformation($"User with id: {id} was found.");
            return StatusCode(StatusCodes.Status200OK, user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] CreateUserInput input)
        {
            _logger.LogInformation($"Create user with username: {input.UserName} started.");
            try
            {
                var insertedUser = await _userService.CreateUserAsync(input);
                return StatusCode(StatusCodes.Status201Created, insertedUser);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogInformation($"Could not create user. Message: {ex.Message}");
                return StatusCode(StatusCodes.Status409Conflict, new {message = ex.Message });
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] EditUserInput userInput)
        {
            _logger.LogInformation($"Update user with id: {id} started.");
            try
            {
                var updatedUser = await _userService.EditUserAsync(id, userInput);
                _logger.LogInformation($"User with id: {id} updated successfully.");

                return StatusCode(StatusCodes.Status200OK, updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status409Conflict);
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"Delete user with id: {id}.");
            try
            {
                var wasDeleted = await _userService.DeleteUserAsync(id);
                if (wasDeleted)
                {
                    _logger.LogInformation($"User with id: {id} deleted.");
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    _logger.LogInformation($"User with id: {id} was not deleted.");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation($"Could not delete user. Error: " + ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
#nullable disable
}
