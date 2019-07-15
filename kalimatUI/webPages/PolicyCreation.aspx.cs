
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using kalimataUI.Library;

using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using kalimataUI.Library.models;


namespace kalimataUI.webPages
{
    public partial class PolicyCreation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Header.InnerText = (string)Session["fullname"] + " - " + (string)Session["contactid"];
        }

        protected void CreatePolicy_OnClick(object sender, EventArgs e)
        {
            NewPolicy();
        }

        private void NewPolicy()
        {
            PolicyCreationModel policyCreationModel = new PolicyCreationModel();

            // Define model
            policyCreationModel.kp_policy = PolicyNameTextBox.Text;

            List<ListItem> policyCheck = new List<ListItem>();
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    if (item.Value == "kp_collison")
                    {
                        policyCreationModel.kp_collision = true;
                    }
                    if (item.Value == "kp_comprehensive")
                    {
                        policyCreationModel.kp_comprehensive = true;
                    }
                    if (item.Value == "kp_liability")
                    {
                        policyCreationModel.kp_liability = true;
                    }
                    if (item.Value == "kp_protected")
                    {
                        policyCreationModel.kp_protected = true;
                    }
                    if (item.Value == "kp_uninsured")
                    {
                        policyCreationModel.kp_uninsured = true;
                    }
                }
            }

            //policyCreationModel.kp_policyholder = Guid.Parse((string)Session["contactID"]);
            policyCreationModel.kp_policyholder = Guid.Parse(HttpContext.Current.Request.Cookies[0].Value);


            string preferredCurrency = (string)Session["transactioncurrencyid"];
            if (preferredCurrency == "CAD")
            {
                policyCreationModel.transactioncurrencyid = Guid.Parse("6f14ded1-1ca2-e911-a9aa-000d3a4025c7");
            }
            else if (preferredCurrency == "USD")
            {
                policyCreationModel.transactioncurrencyid = Guid.Parse("391b1ea8-0fa2-e911-a9b6-000d3a402766");
            }

            // Create Entity and submit it
            CreatePolicy newPolicy = new CreatePolicy();
            newPolicy.PolicyCreation(policyCreationModel);
            Response.Redirect("PolicyDisplay.aspx");
        }
    }
}