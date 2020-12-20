using Digital.Identity.Admin.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digital.Identity.Admin.Services
{
    public interface IUserService
    {
        Task<IList<UserDto>> GetUsersAsync(PagedList pagination);

        Task<UserDto> GetUserAsync(string id);

        Task<UserDto> CreateUserAsync(CreateUserInput input);
        Task<bool> DeleteUserAsync(string id);

        Task<UserDto> EditUserAsync(string id, EditUserInput input);
    }
}
