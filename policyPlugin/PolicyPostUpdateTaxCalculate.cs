using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace kalimataInsurance
{
    public class PolicyPostUpdateTaxCalculate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    if (context.Depth > 1)
                    {
                        return;
                    }
                   

                    Entity currentPolicy = service.Retrieve("kp_policy", entity.Id, new ColumnSet(true));

                    tracingService.Trace("The premium is {0}", ((Money)(currentPolicy.Attributes["kp_premium"])).Value);

                    Guid CATax = new Guid("a9eaee1f-e8a3-e911-a990-000d3a40543b");
                    Entity resultCATax = service.Retrieve("kp_configurations", CATax, new ColumnSet("kp_value"));

                    Guid USTax = new Guid("ca1a452a-e8a3-e911-a990-000d3a40543b");
                    Entity resultUSTax = service.Retrieve("kp_configurations", USTax, new ColumnSet("kp_value"));

                    //check to see if contact exist
                    Entity resultDis;
                    if (currentPolicy.Attributes.Contains("kp_policyholder"))
                    {
                        Guid currentContact = ((EntityReference)currentPolicy.Attributes["kp_policyholder"]).Id;

                        resultDis = service.Retrieve("contact", currentContact, new ColumnSet("address1_country"));
                        tracingService.Trace(resultDis.Attributes["address1_country"].ToString());
                    }
                    //default to US tax if no contact is found
                    else
                    {
                        resultDis = new Entity();
                        resultDis.Attributes.Add("address1_country", "US");
                    }

                    decimal vatTax = 0;
                    if (resultDis.Attributes["address1_country"].ToString().Contains("CA"))
                    {
                        vatTax = decimal.Parse(resultCATax.Attributes["kp_value"].ToString());
                    }
                    else if (resultDis.Attributes["address1_country"].ToString().Contains("US"))
                    {
                        vatTax = decimal.Parse(resultUSTax.Attributes["kp_value"].ToString());
                    }
                    decimal preTax = ((Money)(currentPolicy.Attributes["kp_premium"])).Value;
                    decimal postTax = preTax * (1 + vatTax);
                    Money resultPremiumPostTax = new Money(postTax);
                    currentPolicy.Attributes["kp_ncpremiumposttax"] = resultPremiumPostTax;
                    service.Update(currentPolicy);
                    tracingService.Trace("finish Update");

                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}