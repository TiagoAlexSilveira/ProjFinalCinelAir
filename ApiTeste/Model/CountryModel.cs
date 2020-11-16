using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTeste.Model
{
    public class CountryModel
    {
        public string name { get; set; }
        public string alpha2Code { get; set; }
        public string region { get; set; }
        public string subregion { get; set; }
    }
}
