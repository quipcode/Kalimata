


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace kalimataUI.Library
{
    public class PolicyReferenceModel
    {
        public string kp_policy { get; set; }
        public string kp_policynumber { get; set; }
        public Guid kp_policyholder { get; set; }
        public Guid kp_policyid { get; set; }
    }

    // CRM Connection is in CRMconnect.cs

    public class RetrievePolicy
    {
        public List<PolicyReferenceModel> PolicyRetrieval(Guid contactguid)
        {
            CRMConnect conn = new CRMConnect();

            string fetch2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='kp_policy'>
                                    <attribute name='kp_policy' />
                                    <attribute name='kp_policynumber' />
                                    <attribute name='kp_policyholder' />
                                    <attribute name='kp_policyid' />
                                    <order attribute='kp_policy' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='kp_policyholder' operator='eq' uiname='DÄ«sÂ Pater' uitype='contact' value='{" + contactguid + @"}' />
                                        <condition attribute='kp_policyid' operator='not-null' />
                                        <condition attribute='kp_policy' operator='not-null' />
                                    </filter>
                                </entity>
                            </fetch>";

            EntityCollection dataAll = conn.service.RetrieveMultiple(new FetchExpression(fetch2));

            List<PolicyReferenceModel> listOfPolicies = new List<PolicyReferenceModel>();
            foreach (Entity dataSingle in dataAll.Entities)
            {
                PolicyReferenceModel dataModel = new PolicyReferenceModel();

                dataModel.kp_policy = dataSingle.Attributes["kp_policy"].ToString();
                dataModel.kp_policynumber = dataSingle.Attributes["kp_policynumber"].ToString();
                dataModel.kp_policyholder = new Guid(((EntityReference)dataSingle.Attributes["kp_policyholder"]).Id.ToString());
                dataModel.kp_policyid = dataSingle.Id;

                listOfPolicies.Add(dataModel);
            }
            return listOfPolicies;
        }
    }

    // Creation of Claim is in Create.cs
}