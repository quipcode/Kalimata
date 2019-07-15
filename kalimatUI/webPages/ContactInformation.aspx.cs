using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using kalimataUI.Library;
using kalimataUI.Library.models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Web.SessionState;


namespace kalimataUI.webPages
{
    public partial class ContactInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ChangeContact();
        }

        protected void Update_OnClick(object sender, EventArgs e)
        {
           
            UpdateFields();
            HttpCookie userIdCookie = new HttpCookie("UserID");
            userIdCookie.Value = DropDownList1.SelectedValue.ToString();
            Response.Cookies.Add(userIdCookie);

            HttpCookie userNameCookie = new HttpCookie("UserName");
            userNameCookie.Value = DropDownList1.SelectedItem.Text;
            Response.Cookies.Add(userNameCookie);
        }

        public void NewPage_OnClick(object sender, EventArgs e)
        {
           
            Session.Add("fullname", DropDownList1.SelectedItem.Text);
            Session.Add("contactID", DropDownList1.SelectedValue);
            Session.Add("address1_country", TextBox2.Text);
            Session.Add("transactioncurrencyid", TextBox3.Text);

            Response.Redirect("PolicyDisplay.aspx");
        }

        protected void ChangeContact()
        {
            RetrieveContactGuid startCollection = new RetrieveContactGuid();
            List<ContactGuid> data = startCollection.RetrieveAllContactGuid();

            foreach (ContactGuid contact in data)
            {
                if (DropDownList1.Items.Contains(new ListItem(contact.fullname, contact.contactid.ToString())))
                {
                    continue;
                }
                else
                {
                    DropDownList1.Items.Add(new ListItem(contact.fullname, contact.contactid.ToString()));
                }
            }
        }
        protected void UpdateFields()
        {
            //Guid guid = Guid.Parse(TextBox0.Text);
            string value = DropDownList1.SelectedValue;
            Guid guid = Guid.Parse(value);

            ReadContact data = new ReadContact();
            Contact contact = data.RetrieveContact(guid);

            TextBox1.Text = contact.fullname;
            TextBox2.Text = contact.address1_country;
            TextBox3.Text = contact.transactioncurrencyid;
        }
    }
}