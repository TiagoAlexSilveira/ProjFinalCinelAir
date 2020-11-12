using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class CinelAirCardViewModel : Client
    {

        public DateTime ExpirationDate  { get; set; }

        //**********************
        public int Miles_Number { get; set; }

        public DateTime Validity { get; set; }

        //************************
        public DateTime Start_Date { get; set; }

        public DateTime? End_Date { get; set; }


        public bool isValidated { get; set; }


        public bool wasNominated { get; set; } //preciso desta propriedade porque os que são nomeados gold não podem nomear mais ninguém

        public int StatusId { get; set; }

        public Status Status { get; set; }
        public string ClientStatus { get; set; }

        

    }
}
