using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Models
{
    public class StatusViewModel
    {

        [Required]
        public Client client { get; set; }


        public int StatusId { get; set; }


        public IEnumerable<SelectListItem> Statuses { get; set; }


        [Display(Name ="Miles Status")]
        public int miles_Status_Year { get; set; }

        [Display(Name = "Flights")]
        public int flights_Year { get; set; }



    }
}
