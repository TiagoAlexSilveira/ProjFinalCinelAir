using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class CinelAirCardViewModel
    {
        public int Id { get; set; }
        //deixxar o nome clientId em vez para não confundir com o Id em baixo

        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


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


        //adicionar id do historic status ?? 
        public string UserId { get; set; }

        public User User { get; set; }

        //************************
        public DateTime Start_Date { get; set; }

        public DateTime? End_Date { get; set; }


        public bool isValidated { get; set; }


        public bool wasNominated { get; set; } //preciso desta propriedade porque os que são nomeados gold não podem nomear mais ninguém

        public int StatusId { get; set; }

        public Status Status { get; set; }

    }
}
