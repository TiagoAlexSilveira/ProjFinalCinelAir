using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAPI.Helpers
{
    public class UserHelper : IUserHelper
    {
        /*private readonly UserManager<User> _userManager;
        private readonly DataContext _context;


        public UserHelper(UserManager<User> userManager, DataContext context)
        {

            _userManager = userManager;
            _context = context;
        }

        public async Task<string> clientStatusAsync(string userId)
        {
            var clientStatus = await _context.Historic_Status
                           .Include(d => d.Status)
                           .Where(x => x.UserId == userId)
                           .FirstOrDefaultAsync();

            return clientStatus.Status.Description;
        }

        public User GetUser(int clientNumber)
        {
            User user = _context.Users.FirstOrDefault(x => x.Client_Number == clientNumber);
            return user;
        }


        public async Task<bool> isClientAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);

        }*/

       
    }
}
