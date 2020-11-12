using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class BalanceViewModel
    {
        public DateTime StatusValidity { get; set; }

        public int available_Miles_Status { get; set; }   //milhas válidas depois de feitas o voo ou movimentos(tipo compra antes de voo)

        public DateTime BonusValidity { get; set; }

        public int available_Miles_Bonus { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        [Display(Name = "Movement Type")]
        public string Movement_Type { get; set; }


        public DateTime Date { get; set; }

        public string Description { get; set; }

        [Display(Name = "Balance Miles Status")]
        public int Balance_Miles_Status { get; set; }

        [Display(Name = "Balance Miles Bonus")]
        public int Balance_Miles_Bonus { get; set; }

        public int Miles { get; set; } //Total depois da transação

        public ICollection<Transaction> TransactionList { get; set; } 

        public string ExpiryDateLastMiles { get; set; }

        public int LastMiles { get; set; }

    }
}
