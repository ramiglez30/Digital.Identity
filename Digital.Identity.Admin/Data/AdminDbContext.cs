using Digital.Identity.Admin.Models.EF;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Digital.Identity.Admin.Data
{
    public class AdminDbContext: IdentityDbContext<ApplicationUser>
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options): base(options)
        {

        }
    }
}
