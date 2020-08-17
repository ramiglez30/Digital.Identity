using Digital.Identity.Admin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Digital.Identity.Admin.Data
{
    public class AdminDbContext: IdentityDbContext<ApplicationUser>
    {
        public AdminDbContext(DbContextOptions options): base(options)
        {

        }
    }
}
