using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Models
{
    public class RegisterNewEmployeeViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

       
        public string Email { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string StreetAdress { get; set; }


        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string PhoneNumber { get; set; }


        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city")]
        public int CityId { get; set; }


        public IEnumerable<SelectListItem> Cities { get; set; }


        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        public int CountryId { get; set; }


        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }


        public IEnumerable<SelectListItem> Countries { get; set; }


        public IEnumerable<SelectListItem> Category { get; set; }

        [Required]
        public string CategoryId { get; set; }


        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }

        [Required]
        [Display(Name = "Tax Number")]
        public int TaxNumber { get; set; }

        [Required]
        [Display(Name = "Identification Number")]
        public string Identification { get; set; }

        
        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime JoinDate { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? DateofBirth { get; set; }

    }
}
