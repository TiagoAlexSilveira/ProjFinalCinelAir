using System;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Historic_Status : IEntity
    {

        public int Id { get; set; }

        public DateTime Start_Date { get; set; }

        public DateTime? End_Date { get; set; }


        public bool isValidated { get; set; }


        public bool wasNominated { get; set; } //preciso desta propriedade porque os que são nomeados gold não podem nomear mais ninguém

        public int StatusId { get; set; }

        public Status Status { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }




    }
}
