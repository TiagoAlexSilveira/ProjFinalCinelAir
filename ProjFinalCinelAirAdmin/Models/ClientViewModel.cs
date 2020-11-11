using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Models
{
    public class ClientViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Client Number")]
        public int ClientNumber { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }


        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }


        public string CityName { get; set; }

        public City City { get; set; }


        [Required]
        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city")]
        public int CityId { get; set; }


        public IEnumerable<SelectListItem> Cities { get; set; }

        [Required]
        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        public int CountryId { get; set; }


        public IEnumerable<SelectListItem> Countries { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DateofBirth { get; set; }

        [Required]
        [Display(Name = "Tax Number")]
        public int TaxNumber { get; set; }

        [Required]
        [Display(Name = "Citizen Card Number")]
        public string Identification { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{this.FirstName} {this.LastName}";

        [Required]
        [Display(Name = "Client Validated")]
        public bool isValidated { get; set; }

    
    }
}
