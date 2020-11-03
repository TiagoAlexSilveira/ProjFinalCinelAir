using Microsoft.AspNetCore.Identity;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAPI.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;


        public UserHelper(UserManager<User> userManager, DataContext context)
        {

            _userManager = userManager;
            _context = context;
        }

        public User GetUser(int nif)
        {
            User user = _context.Users.FirstOrDefault(x => x.TaxNumber == nif);
            return user;
        }


        public async Task<bool> isClientAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);

        }
    }
}
