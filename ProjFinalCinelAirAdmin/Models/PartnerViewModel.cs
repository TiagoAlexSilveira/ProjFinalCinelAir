using Microsoft.AspNetCore.Http;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Models
{
    public class PartnerViewModel : Partner
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
