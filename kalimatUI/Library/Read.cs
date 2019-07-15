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
using Microsoft.Xrm.Sdk.Query;

using System.Text.RegularExpressions;

namespace kalimataUI.Library
{
    public class PolicyRead : CRMConnect
    {
        CRMConnect conn = new CRMConnect();
        public List<PolicyModel> GetPolicies(/*PolicyModel policy*/)
        {
            //string userGuidString = HttpContext.Current.Session["contactid"].ToString();
            //Session.Add("contactID", DropDownList1.SelectedValue);
            //Guid usersGuid = Guid.Parse((string)Session["contactid"]);


            //HttpContext context = HttpContext.Current;
            //string usersGuid = context.Session["FirstName"].ToString();
            string userActualStringGuid = "";
            string userGuidValue = "";
            string testUserGuid = "B6F1EFEE-09A4-E911-A990-000D3A40543B";
            if (HttpContext.Current.Request.Cookies[0] != null)
            {
                userGuidValue = HttpContext.Current.Request.Cookies[0].Value;
                    
              string lines = Regex.Split(userGuidValue, "(?<!=)=(?!=)").ToString();
                //HttpCookie lineCookie = new HttpCookie("lengthOfSplit");
                //lineCookie.Value = lines.Count().ToString();


                userActualStringGuid = lines.Substring(6);
                
            }


            string fetchUsersPolicies = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='kp_policy'>
                <attribute name='kp_policy' />
                <attribute name='createdon' />
                <attribute name='kp_premium' />
                <attribute name='transactioncurrencyid' />
                <attribute name='kp_policynumber' />
                <attribute name='kp_policyholder' />
                <attribute name='kp_ncpremiumposttax' />
                <attribute name='kp_policyid' />
                <attribute name='kp_uninsured' />
                <attribute name='kp_protected' />
                <attribute name='kp_liability' />
                <attribute name='kp_comprehensive' />
                <attribute name='kp_collision' />
                <order attribute='kp_policy' descending='false' />
                <filter type='and'>
                  <condition attribute='kp_policyholder' operator='eq' uitype='contact' value='{" + userGuidValue + @"}'
             />
                </filter>
              </entity>
            </fetch>";
    
            EntityCollection usersPolicyEntities = conn.service.RetrieveMultiple(new FetchExpression(fetchUsersPolicies));

            List<PolicyModel> listOfPolicies = new List<PolicyModel>();

            foreach (Entity individualPolicy in usersPolicyEntities.Entities)
            {
                PolicyModel userIndividualPolicy = new PolicyModel();
                userIndividualPolicy.kp_liability = individualPolicy.Attributes["kp_liability"].ToString();
                userIndividualPolicy.kp_collision = individualPolicy.Attributes["kp_collision"].ToString();
                userIndividualPolicy.kp_comprehensive = individualPolicy.Attributes["kp_comprehensive"].ToString();
                userIndividualPolicy.kp_protected = individualPolicy.Attributes["kp_protected"].ToString();
                userIndividualPolicy.kp_uninsured = individualPolicy.Attributes["kp_uninsured"].ToString();
                userIndividualPolicy.kp_premium = ((Money)(individualPolicy.Attributes["kp_premium"])).Value.ToString();
                userIndividualPolicy.kp_policynumber = individualPolicy.Attributes["kp_policynumber"].ToString();
                userIndividualPolicy.kp_policy = individualPolicy.Attributes["kp_policy"].ToString();
                userIndividualPolicy.kp_policyholder = ((EntityReference)individualPolicy.Attributes["kp_policyholder"]).Id;
                userIndividualPolicy.transactioncurrencyid = ((EntityReference)individualPolicy.Attributes["transactioncurrencyid"]).Id;
                listOfPolicies.Add(userIndividualPolicy);
            }

            return listOfPolicies;
        }
    }


    public class RetrieveContactGuid
    {
        public List<ContactGuid> RetrieveAllContactGuid()
        {
            CRMConnect conn = new CRMConnect();

            string fetch2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='contact'>
                                        <attribute name='fullname' />
                                        <attribute name='contactid' />
                                        <attribute name='kp_kalimatainsurance' />
                                        <filter type='and'>
                                            <condition attribute='kp_kalimatainsurance' operator='eq' value='4' />
                                        </filter>
                                    </entity>
                            </fetch>";

            EntityCollection dataAll = conn.service.RetrieveMultiple(new FetchExpression(fetch2));

            List<ContactGuid> listOfContacts = new List<ContactGuid>();

            foreach (Entity user in dataAll.Entities)
            {
                ContactGuid contactguid = new ContactGuid();

                contactguid.fullname = user.Attributes["fullname"].ToString();
                contactguid.contactid = user.Id;

                listOfContacts.Add(contactguid);
            }

            return listOfContacts;
        }


    }
    public class ReadContact
    {
        public Contact RetrieveContact(Guid guid)
        {
            // B6F1EFEE-09A4-E911-A990-000D3A40543B

            // New Connection Made
            CRMConnect conn = new CRMConnect();

            // Fetch Expression for contact
            string fetch2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                        <entity name='contact'>
                                        <attribute name='fullname' />
                                        <attribute name='contactid' />
                                        <attribute name='transactioncurrencyid' />
                                        <attribute name='address1_country' />
                                        <order attribute='fullname' descending='false' />
                                        <filter type='and'>
                                            <condition attribute='fullname' operator='not-null' />
                                            <condition attribute='address1_country' operator='not-null' />
                                            <condition attribute='transactioncurrencyid' operator='not-null' />
                                            <condition attribute='contactid' operator='eq' uiname='DeaÂ Tacita' uitype='contact' value='{" + guid + @"}' />
                                        </filter>
                                    </entity>
                                </fetch>";

            EntityCollection dataAll = conn.service.RetrieveMultiple(new FetchExpression(fetch2));

            // Usable format
            Contact contact = new Contact();
            foreach (Entity user in dataAll.Entities)
            {
                contact.fullname = user.Attributes["fullname"].ToString();
                contact.address1_country = user.Attributes["address1_country"].ToString();

                string currencyID = ((EntityReference)user.Attributes["transactioncurrencyid"]).Id.ToString();
                if (currencyID == "6f14ded1-1ca2-e911-a9aa-000d3a4025c7")
                {
                    contact.transactioncurrencyid = "CAD";
                }
                else if (currencyID == "391b1ea8-0fa2-e911-a9b6-000d3a402766")
                {
                    contact.transactioncurrencyid = "USD";
                }
            }
            return contact;
        }
    }

}