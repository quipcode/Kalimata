using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kalimataUI.Library.models
{
    public class Contact
    {
        public string fullname { get; set; }
        public string address1_country { get; set; }
        public string transactioncurrencyid { get; set; }
    }
    public class ContactGuid
    {
        public Guid contactid { get; set; }
        public string fullname { get; set; }
    }
}