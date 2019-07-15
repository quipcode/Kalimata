using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using kalimataUI.Library.models;
using kalimataUI.Library;
using System.Collections;

namespace kalimataUI.webPages
{
    public partial class PolicyDisplay : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string contactid = HttpContext.Current.Request.Cookies[0].Value;
            //string contactid = (string)Session["contactID"];
            Guid guidid = new Guid(contactid);
            ChangePolicy(guidid);
            string userFullName = HttpContext.Current.Request.Cookies.Get("UserName").Value;
            PolicyRead listOfUsersPolicy = new PolicyRead();

            List<PolicyModel> policiesList = listOfUsersPolicy.GetPolicies();
            
            foreach (PolicyModel policy in policiesList)
            {
 
                TableRow row = new TableRow();
                TableCell cell1 = new TableCell();
                cell1.Text = policy.kp_liability;
                row.Cells.Add(cell1);
                TableCell cell2 = new TableCell();
                cell2.Text = policy.kp_collision;
                row.Cells.Add(cell2);
                TableCell cell3 = new TableCell();
                cell3.Text = policy.kp_comprehensive;
                row.Cells.Add(cell3);
                TableCell cell4 = new TableCell();
                cell4.Text = policy.kp_protected;
                row.Cells.Add(cell4);
                TableCell cell5 = new TableCell();
                cell5.Text = policy.kp_uninsured;
                row.Cells.Add(cell5);
                TableCell cell6 = new TableCell();
                cell6.Text = policy.kp_premium;
                row.Cells.Add(cell6);
                TableCell cell7 = new TableCell();
                cell7.Text = policy.kp_policynumber;
                row.Cells.Add(cell7);
                TableCell cell8 = new TableCell();
                cell8.Text = policy.kp_policy;
                row.Cells.Add(cell8);
                TableCell cell9 = new TableCell();
                cell9.Text = userFullName;
                //cell9.Text = policy.kp_policyholder.ToString();
                row.Cells.Add(cell9);
                TableCell cell10 = new TableCell();
                cell10.Text = policy.transactioncurrencyid.ToString();
                row.Cells.Add(cell10);

                Table1.Rows.Add(row);
            }

        }

        public void PolicyCreatePage_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("PolicyCreation.aspx");
        }
        public void CreateClaim_OnClick(object sender, EventArgs e)
        {
            //Session.Add("contactID", DropDownList1.SelectedValue);


            Response.Redirect("ClaimCreation.aspx");
        }

        public void ViewSpecificClaim(object sender, EventArgs e)
        {

            
            HttpCookie policyIdCookie = new HttpCookie("PolicyID");
            policyIdCookie.Value = DropDownList1.SelectedValue.ToString();
            Response.Cookies.Add(policyIdCookie);
           

       
            Response.Redirect("ClaimsDisplay.aspx");
        
        }
        private void ChangePolicy(Guid guid)
        {
            PolicyReferenceModel startCollection = new PolicyReferenceModel();
            RetrievePolicy obj = new RetrievePolicy();
            List<PolicyReferenceModel> data = obj.PolicyRetrieval(guid);

            foreach (PolicyReferenceModel model in data)
            {
                if (DropDownList1.Items.FindByValue(model.kp_policyid.ToString()) != null)
                {
                    continue;
                }
                else
                {
                    string itemName = model.kp_policy + "   -   " + model.kp_policynumber;
                    DropDownList1.Items.Add(new ListItem(itemName, model.kp_policyid.ToString()));
                }
            }
        }
    }
}