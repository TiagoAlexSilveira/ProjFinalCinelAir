using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class User : IdentityUser
    {

        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }



        public int Client_Number { get; set; }



        [Display(Name = "Image")]
        public string ImageUrl { get; set; }


        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }


        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DateofBirth { get; set; }


        [Display(Name = "Tax Number")]
        public int TaxNumber { get; set; }


        [Display(Name = "Citizen Card Number")]
        public string Identification { get; set; }


        public DateTime? JoinDate { get; set; }


        [Display(Name = "Miles Status")]
        public int Miles_Status { get; set; }


        [Display(Name = "Miles Bonus")]
        public int Miles_Bonus { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{this.FirstName} {this.LastName}";

    }
}
