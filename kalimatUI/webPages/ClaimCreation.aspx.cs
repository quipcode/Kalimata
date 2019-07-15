using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using kalimataUI.Library;
using kalimataUI.Library.models;

namespace kalimataUI.webPages
{
    public partial class ClaimCreation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string contactid = HttpContext.Current.Request.Cookies[0].Value;
            //string contactid = (string)Session["contactID"];
            Guid guidid = new Guid(contactid);
            ChangePolicy(guidid);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ClaimCreationModel cla = new ClaimCreationModel();
            // cla.kp_claimname = TextBox1.Text;
            cla.kp_claim = TextBox1.Text;
            cla.kp_claimcontact = new Guid(HttpContext.Current.Request.Cookies.Get("UserID").Value);
            cla.kp_claimpolicy = new Guid(DropDownList1.SelectedValue);

            CreateClaim creation = new CreateClaim();
            creation.ClaimCreate(cla);
            Response.Redirect("PolicyDisplay.aspx");
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