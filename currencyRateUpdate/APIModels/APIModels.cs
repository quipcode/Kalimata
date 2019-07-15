using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace currencyRateUpdate.APIModels
{
    public class Message
    {
        public bool success { get; set; }
        public Rates rates { set; get; }
    }
    public class Rates
    {
        public string USD { get; set; }
        public string CAD { get; set; }
    }
}
