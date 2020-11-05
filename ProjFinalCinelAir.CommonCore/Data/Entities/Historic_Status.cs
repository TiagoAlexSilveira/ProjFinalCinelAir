using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Historic_Status : IEntity
    {

        public int Id { get; set; }

        public DateTime Start_Date { get; set; }

        public DateTime? End_Date { get; set; }


        public bool isValidated { get; set; }

        
        public int StatusId { get; set; }

        public Status Status { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }




    }
}
