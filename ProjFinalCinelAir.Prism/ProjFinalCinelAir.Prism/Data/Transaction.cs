using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.Prism.Data
{
    public class Transaction
    {

        public int Id { get; set; }

   
        public string Movement_Type { get; set; }


        public DateTime Date { get; set; }

        public string Description { get; set; }

  
        public int Balance_Miles_Status { get; set; }


        public int Balance_Miles_Bonus { get; set; }

        public int Miles { get; set; } //Total depois da transação


    }
}
