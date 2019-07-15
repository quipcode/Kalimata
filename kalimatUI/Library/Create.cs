

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using kalimataUI.Library.models;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System.Runtime.Serialization.Json;

namespace kalimataUI.Library
{
    public class CreatePolicy
    {
        public void PolicyCreation(PolicyCreationModel policyCreationModel)
        {
            CRMConnect conn = new CRMConnect();

            Entity pol = new Entity("kp_policy");

            //string policy = policyCreationModel.kp_policyholder.ToString();
            EntityReference policyholder = new EntityReference("contact", policyCreationModel.kp_policyholder);
            EntityReference preferredCurrencyID = new EntityReference("contact", policyCreationModel.transactioncurrencyid);


            pol.Attributes.Add("kp_policyholder", policyholder);
            pol.Attributes.Add("transactioncurrencyid", preferredCurrencyID);
            pol.Attributes.Add("kp_policy", policyCreationModel.kp_policy);

            Guid createdPolicy = conn.service.Create(pol);
            Entity newPolicy = conn.service.Retrieve("kp_policy", createdPolicy, new ColumnSet());

            //newPolicy.Attributes.Add("kp_policy", policyCreationModel.kp_policy);

            if (policyCreationModel.kp_collision == true)
            {
                newPolicy.Attributes.Add("kp_collision", policyCreationModel.kp_collision);
                //newPolicy.Attributes["kp_collision"] = policyCreationModel.kp_collision;
            }
            if (policyCreationModel.kp_comprehensive ==  true)
            {
                newPolicy.Attributes.Add("kp_comprehensive", policyCreationModel.kp_comprehensive);
                //newPolicy.Attributes["kp_comprehensive"] = policyCreationModel.kp_comprehensive;
            }
            if (policyCreationModel.kp_liability == true)
            {
                newPolicy.Attributes.Add("kp_liability", policyCreationModel.kp_liability);
                //newPolicy.Attributes["kp_liability"] = policyCreationModel.kp_liability;
            }
            if (policyCreationModel.kp_protected == true)
            {
                newPolicy.Attributes.Add("kp_protected", policyCreationModel.kp_protected);
                //newPolicy.Attributes["kp_protected"] = policyCreationModel.kp_protected;
            }
            if (policyCreationModel.kp_uninsured == true)
            {
                newPolicy.Attributes.Add("kp_uninsured", policyCreationModel.kp_uninsured);
                //newPolicy.Attributes["kp_uninsured"] = policyCreationModel.kp_uninsured;
            }

            conn.service.Update(newPolicy);
        }
    }
    public class CreateClaim
    {
        public void ClaimCreate(ClaimCreationModel kp_claim)
        {
            //Entity record = new Entity("kp_claim");
            //record.Attributes.Add("kp_claim", kp_claim.kp_claim);
            //record.Attributes.Add("kp_claimcontact", (EntityReference)kp_claim.kp_claimcontact);
            //record.Attributes.Add("kp_claimpolicy", kp_claim.kp_claimpolicy);
            //CRMConnect conn = new CRMConnect();
            //conn.service.Create(record);

            Entity record = new Entity("kp_claim");
            record.Attributes.Add("kp_claim", kp_claim.kp_claim);

            EntityReference contactReference = new EntityReference("contact", kp_claim.kp_claimcontact);
            record.Attributes.Add("kp_claimcontact", contactReference);

            // Convert to entity reference
            EntityReference policyReference = new EntityReference("kp_policy", kp_claim.kp_claimpolicy);
            record.Attributes.Add("kp_claimpolicy", policyReference);

            CRMConnect conn = new CRMConnect();
            conn.service.Create(record);
        }
    }
}