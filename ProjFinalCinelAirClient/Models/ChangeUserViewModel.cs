using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Models
{
    public class ChangeUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }



        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }



        [MaxLength(100, ErrorMessage = "The field {0} can only contain {1} characters.")]
        public string Address { get; set; }


        [MaxLength(100, ErrorMessage = "The field {0} can only contain {1} characters.")]
        public string PhoneNumber { get; set; }



    }
}
