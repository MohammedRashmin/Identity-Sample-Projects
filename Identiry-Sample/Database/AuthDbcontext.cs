using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identiry_Sample.Database
{
    public class AuthDbcontext : IdentityDbContext<Customer>
    {
        public AuthDbcontext(DbContextOptions options) : base(options)
        {
        }
    }
}
