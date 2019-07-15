using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Query;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using kalimataUI.Library;
//using Contact_Web_Page.Library.Models;


namespace kalimataUI.Library
{

    public class CRMConnect
    {
        public CrmServiceClient service = null;
        public CRMConnect()
        {
            string crmconnectionstring = "AuthType=Office365;Username = websservice@kalimata.onmicrosoft.com;Password =FPp3PWBk;Url = https://kalimata.crm7.dynamics.com";
            service = new CrmServiceClient(crmconnectionstring);
        }


    }
}