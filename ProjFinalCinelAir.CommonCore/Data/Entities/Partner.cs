using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Partner
    {
        public string Name { get; set; }

        public string Description { get; set; }


        //link para website
        public string Contact {get; set;}


        //Imagem do logo da companhia
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        //TODO:por isto na base de dados
        //se tiver tempo criar páginas das companhias
    }
}
