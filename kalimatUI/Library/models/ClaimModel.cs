using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kalimataUI.Library.models
{
    public class ClaimCreationModel
    {
        public string kp_claim { get; set; }
        public Guid kp_claimcontact { get; set; }
        public Guid kp_claimpolicy { get; set; }
    }
}