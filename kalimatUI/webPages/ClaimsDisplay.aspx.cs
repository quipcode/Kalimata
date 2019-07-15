using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using kalimataUI.Library;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;


namespace kalimataUI.webPages
{
    public partial class ClaimsDisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CRMConnect connection = new CRMConnect();
            //string policyGuidValue = HttpContext.Current.Request.Cookies[2].Value;
            string policyGuidValue = HttpContext.Current.Request.Cookies.Get("PolicyID").Value;
    

            string fetchclaim = @"<fetch mapping='logical' version='1.0' distinct='false' output-format='xml-platform'>
  <entity name='kp_claim'>
    <attribute name='kp_claimid' />
    <attribute name='createdon' />
    <attribute name='kp_claimsid' />
    <attribute name='ownerid' />
    <attribute name='kp_claim' />
    <attribute name='kp_claimcontact' />
    <attribute name='kp_claimpolicy' />
    <filter type='and'>
      <condition value='{"+ policyGuidValue + @"}' attribute='kp_claimpolicy' operator='eq' uitype='kp_policy' />
    </filter>
  </entity>
</fetch>";
            EntityCollection claimsList = connection.service.RetrieveMultiple(new FetchExpression(fetchclaim));
            string userFullName = HttpContext.Current.Request.Cookies.Get("UserName").Value;

            foreach (Entity claim in claimsList.Entities)
            {

                TableRow row = new TableRow();
                TableCell cell1 = new TableCell();
                cell1.Text = claim.Attributes["kp_claimid"].ToString();
                row.Cells.Add(cell1);
                TableCell cell2 = new TableCell();
                cell2.Text = claim.Attributes["kp_claim"].ToString();
                row.Cells.Add(cell2);
                TableCell cell3 = new TableCell();
                //cell3.Text = ((EntityReference)claim.Attributes["kp_claimcontact"]).Id.ToString();
                cell3.Text = userFullName;
                row.Cells.Add(cell3);
                TableCell cell4 = new TableCell();
                cell4.Text = ((EntityReference)claim.Attributes["kp_claimpolicy"]).Id.ToString();
                row.Cells.Add(cell4);


                ClaimsTable.Rows.Add(row);
            }

        }
    }
}