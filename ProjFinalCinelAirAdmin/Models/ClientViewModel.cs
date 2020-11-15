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
        public Client client { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        public int StatusId { get; set; }

        public Historic_Status Historic_Status { get; set; }


        public int Historic_StatusId { get; set; }

        public IEnumerable<SelectListItem> Status { get; set; }


        public string CityName { get; set; }

        public City City { get; set; }



        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city")]
        public int CityId { get; set; }


        public IEnumerable<SelectListItem> Cities { get; set; }


        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        public int CountryId { get; set; }


        public IEnumerable<SelectListItem> Countries { get; set; }

    }
}
