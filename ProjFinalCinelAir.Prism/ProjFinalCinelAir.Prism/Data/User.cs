using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.Prism.Data
{
    public class User
    {
       
        public string FirstName { get; set; }


      
        public string LastName { get; set; }


       
        public string ImageUrl { get; set; }


      
        public string StreetAddress { get; set; }


      
        public string PostalCode { get; set; }


        public int CityId { get; set; }


        public City City { get; set; }


     
        public DateTime? DateofBirth { get; set; }


       
        public int TaxNumber { get; set; }


     
        public string Identification { get; set; }


        public DateTime JoinDate { get; set; }


     
        public string FullName => $"{this.FirstName} {this.LastName}";

        public bool isActive { get; set; }

    }
}
