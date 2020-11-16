using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class RegisterNewUserViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //TODO: regular expressions para os valores [RegularExpression("")]
        [Required]     
        [StringLength(40, ErrorMessage = "{0} should have between {2} and {1} characters", MinimumLength = 1)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        public int Client_Number { get; set; }


        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "{0} should have between {2} and {1} characters", MinimumLength = 4)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DateofBirth { get; set; }

        [Required]
        [Display(Name = "Tax Number")]
        public int TaxNumber { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "{0} should have {2} numbers", MinimumLength = 8)]
        [Display(Name = "Citizen Card Number")]
        public string Identification { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? JoinDate { get; set; }


        [Display(Name = "Miles Status")]
        public int Miles_Status { get; set; }


        [Display(Name = "Miles Bonus")]
        public int Miles_Bonus { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{this.FirstName} {this.LastName}";


        public string UserId { get; set; }

        public User User { get; set; }

        public bool isClientNumberConfirmed { get; set; }

        public int AnnualMilesBought { get; set; } //limite anual de milhas compradas pelo cliente (máx 20000 milhas por ano)
        public int AnnualMilesTransfered { get; set; }
        public int AnnualMilesConverted { get; set; }

        public int AnnualMilesExtended { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }

    }
}
