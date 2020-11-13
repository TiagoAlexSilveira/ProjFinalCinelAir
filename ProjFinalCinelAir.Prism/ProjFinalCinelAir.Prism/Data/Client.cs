using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.Prism.Data
{
    public class Client
    {
        public int Id { get; set; }


       
        public string FirstName { get; set; }


       
        public string LastName { get; set; }


        
        public string Email { get; set; }


       
        public string PhoneNumber { get; set; }


        public int Client_Number { get; set; }


       
        public string ImageUrl { get; set; }


       
        public string StreetAddress { get; set; }


       
        public string PostalCode { get; set; }


     
        public DateTime DateofBirth { get; set; }


      
        public int TaxNumber { get; set; }


     
        public string Identification { get; set; }


        public DateTime? JoinDate { get; set; }


       
        public int Miles_Status { get; set; }


       
        public int Miles_Bonus { get; set; }


       
        public string FullName => $"{this.FirstName} {this.LastName}";


        public string UserId { get; set; }

        public User User { get; set; }

        public bool isClientNumberConfirmed { get; set; }

        public int AnnualMilesBought { get; set; } //limite anual de milhas compradas pelo cliente (máx 20000 milhas por ano)
        public int AnnualMilesTransfered { get; set; }
        public int AnnualMilesConverted { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }
    }
}
