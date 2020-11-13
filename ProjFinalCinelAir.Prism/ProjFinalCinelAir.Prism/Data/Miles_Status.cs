using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.Prism.Data
{
    public class Miles_Status
    {

        public int Id { get; set; }

        public int Miles_Number { get; set; }  //recebe depois de um voo

        public DateTime Validity { get; set; }

        public int available_Miles_Status { get; set; }   //milhas válidas depois de feitas o voo ou movimentos(tipo compra antes de voo)

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
