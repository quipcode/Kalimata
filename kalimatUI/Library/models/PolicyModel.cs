using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;


namespace kalimataUI.Library.models
{
    public class PolicyModel
    {
        public string kp_liability { get; set; }
        public string kp_collision { get; set; }
        public string kp_comprehensive { get; set; }
        public string kp_protected { get; set; }
        public string kp_uninsured { get; set; }
        public string kp_premium { get; set; }
        public string kp_policynumber { get; set; }
        public string kp_policy { get; set; }
        public Guid kp_policyholder { get; set; }
        public Guid transactioncurrencyid { get; set; }
    }

    public class PolicyCreationModel
    {
        public string kp_policy { get; set; }
        public bool kp_collision { get; set; }
        public bool kp_comprehensive { get; set; }
        public bool kp_liability { get; set; }
        public bool kp_protected { get; set; }
        public bool kp_uninsured { get; set; }
        public Guid kp_policyholder { get; set; }
        public Guid transactioncurrencyid { get; set; }
    }

}