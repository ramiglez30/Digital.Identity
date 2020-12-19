using AutoMapper;
using Digital.Identity.Admin.Models.Api;
using Digital.Identity.Admin.Models.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital.Identity.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper; 

        public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateUserAsync(CreateUserInput input)
        {
            var user = new ApplicationUser { UserName = input.UserName, Email = input.Email };
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                throw new ArgumentException($"Some errors ocurred when trying to create the user. Errors: {result.Errors.Aggregate("", (acc,next)=> $"{acc} .- {next.Description}", result => result)}");
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) {
                throw new KeyNotFoundException($"User was not found with id: {id}.");
            }

            await _userManager.DeleteAsync(user);
        }

        public async Task<UserDto> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) _logger.LogInformation($"Could not find user with id: {id}"); 

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IList<UserDto>> GetUsersAsync(PagedList pagination)
        {
            var usersQuery = _userManager.Users.AsQueryable();

            if (pagination != null && pagination.PageTotal > 0) usersQuery = usersQuery.Skip(pagination.Skipped()).Take(pagination.PageTotal);

            var users = await usersQuery.ToListAsync();
            if(users.Count > 0) _logger.LogInformation($"Could not find any user for this request. page number: {pagination?.PageNumber}, and total per page: {pagination?.PageTotal}");

            return _mapper.Map<IList<UserDto>>(users);
        }
    }
}
