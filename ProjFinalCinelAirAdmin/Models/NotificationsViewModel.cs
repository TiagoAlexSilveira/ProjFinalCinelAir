using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Controllers;
using ProjFinalCinelAirAdmin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Models
{
    public class NotificationsViewModel
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public string SuperUserReply { get; set; }

  

        //public ICollection<Notification> NotificationList { get; set; }

    }
}
