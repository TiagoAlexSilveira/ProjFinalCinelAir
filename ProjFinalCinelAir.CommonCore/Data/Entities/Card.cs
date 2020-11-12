using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.CommonCore.Data.Entities
{
    public class Card : IEntity
    {
        public int Id { get; set; }
        //TODO: adicionar à base de dados

        public DateTime ExpirationDate { get; set; }

    }
}
