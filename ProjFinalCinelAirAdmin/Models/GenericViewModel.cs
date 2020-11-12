using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Models
{
    public class GenericViewModel
    {

        public List<Client> Clients { get; set; }

        public List<Partner> Partners { get; set; }

        public List<AwardTicket> AwardTickets { get; set; }

        public List<NotificationsViewModel> Notifications { get; set; }

 

        public GenericViewModel()
        {
            Clients = new List<Client>();
            Partners = new List<Partner>();
            AwardTickets = new List<AwardTicket>();
            Notifications = new List<NotificationsViewModel>();
        }
        
    }
}
