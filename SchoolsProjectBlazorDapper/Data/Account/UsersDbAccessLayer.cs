using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SchoolsProjectBlazorDapper.Data.Account
{
    public class UsersDbAccessLayer(DbContextOptions<UsersDbAccessLayer> options) : IdentityDbContext<User>(options)
    {
    }
}
