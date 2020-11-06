using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Image")]
        public string ImageUrl { get; set; }


        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }


        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }


        public int CityId { get; set; }


        public City City { get; set; }



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DateofBirth { get; set; }


        [Display(Name = "Tax Number")]
        public int TaxNumber { get; set; }


        [Display(Name = "Citizen Card Number")]
        public string Identification { get; set; }


        public DateTime JoinDate { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{this.FirstName} {this.LastName}";


        public string UserId { get; set; }

        public User User { get; set; }

    }
}
