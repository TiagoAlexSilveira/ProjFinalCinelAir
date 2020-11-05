using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Collections.Generic;
using System.Linq;


namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context)
        {
            _context = context;
        }



        public Client GetClientByUserId(string id)
        {
            var client = _context.Client.FirstOrDefault(u => u.UserId == id);

            return client;
        }


        public Client GetClientByUserEmail(string email)
        {
            var client = _context.Client.FirstOrDefault(e => e.User.Email == email);

            return client;
        }


        public User GetUserByClientId(int id)
        {
            var client = _context.Client.Find(id);
            var user = _context.Users.Find(client.UserId);

            return user;
        }

        public IEnumerable<SelectListItem> GetComboClients()
        {
            var list = _context.Client.Select(b => new SelectListItem
            {
                Text = b.FullName + " - " + b.PhoneNumber,
                Value = b.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a client)",
                Value = "0"
            });

            return list;
        }






    }
}
