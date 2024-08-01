using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolsProjectBlazorDapper.Data.Account;

namespace SchoolsProjectBlazorDapper.Data.Account
{
    public class UsersDbAccessLayer(DbContextOptions<UsersDbAccessLayer> options) : IdentityDbContext<User>(options)
    {
        public List<UserViewModel> GetUsersRoles()
        {
            var usersWithRoles = (from user in Users
                                  join userRole in UserRoles on user.Id equals userRole.UserId
                                  join role in Roles on userRole.RoleId equals role.Id
                                  select new UserViewModel
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      Role = role.Name
                                  });

            return usersWithRoles.ToList();
        }
    }
}
