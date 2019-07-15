using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace policy
{


    public class PolicyPostCreate : IPlugin
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
                Entity policy = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {

                    string fetchAllPoliciesPriceList = @"<fetch mapping='logical' output-format='xml-platform' version='1.0' distinct='false'>
                      <entity name='productpricelevel'>
                        <attribute name='productid' />
                        <attribute name='uomid' />
                        <attribute name='productpricelevelid' />
                        <attribute name='transactioncurrencyid' />
                        <attribute name='amount' />
                        <attribute name='pricelevelidname' /> 
                        <order descending='false' attribute='productid' />
                        <filter type='and'>
                          <condition value='%InsurancePolicy' attribute='pricelevelidname' operator='like' />
                        </filter>
                        <link-entity name='product' to='productid' from='productid' alias='a_04a3781107a2e911a9b6000d3a402766' link-type='outer' visible='false'>
                          <attribute name='name' />
                        </link-entity>
                      </entity>
                    </fetch>";

                    decimal collisionUS = 0;
                    decimal collisionCA = 0;
                    decimal comprehensiveUS = 0;
                    decimal comprehensiveCA = 0;
                    decimal liabilityUS = 0;
                    decimal liabilityCA = 0;
                    decimal personalUS = 0;
                    decimal personalCA = 0;
                    decimal uninsuredUS = 0;
                    decimal uninsuredCA = 0;

                    tracingService.Trace("Retrived PriceList Success");

                    EntityCollection policyPriceList = service.RetrieveMultiple(new FetchExpression(fetchAllPoliciesPriceList));
                    for (int i = 0; i < policyPriceList.Entities.Count; i++)
                    {
                        Entity currentEntity = policyPriceList.Entities.ElementAt(i);
                        if (i == 0)
                        {
                            collisionUS = ((Money)currentEntity.Attributes["amount"]).Value;
                        }
                        else if (i == 1)
                        {
                            collisionCA = ((Money)currentEntity.Attributes["amount"]).Value;
                        }
                        else if (i == 2)
                        {
                            comprehensiveUS = ((Money)currentEntity.Attributes["amount"]).Value;
                        }
                        else if (i == 3)
                        {
                            comprehensiveCA = ((Money)currentEntity.Attributes["amount"]).Value;
                        }
                        else if (i == 4)
                        {
                            liabilityUS = ((Money)currentEntity.Attributes["amount"]).Value;
                        }
                        else if (i == 5)
                        {
                            liabilityCA = ((Money)currentEntity.Attributes["amount"]).Value;
                        }
                        else if (i == 6)
                        {
                            personalUS = ((Money)currentEntity.Attributes["amount"]).Value;
                        }
                        else if (i == 7)
                        {
                            personalCA = ((Money)currentEntity.Attributes["amount"]).Value;
                        }
                        else if (i == 8)
                        {
                            uninsuredUS = ((Money)currentEntity.Attributes["amount"]).Value;
                        }
                        else if (i == 9)
                        {
                            uninsuredCA = ((Money)currentEntity.Attributes["amount"]).Value;
                        }

                    }


                    tracingService.Trace("Updating Attribute");

                    if (policy.Attributes.Contains("kp_collisionpricecad"))
                    {
                        policy.Attributes["kp_collisionpricecad"] = collisionCA;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_collisionpricecad", collisionCA);
                    }

                    if (policy.Attributes.Contains("kp_collisionpriceus"))
                    {
                        policy.Attributes["kp_collisionpriceus"] = collisionUS;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_collisionpriceus", collisionUS);
                    }

                    if (policy.Attributes.Contains("kp_comprehensivepricecad"))
                    {
                        policy.Attributes["kp_comprehensivepricecad"] = comprehensiveCA;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_comprehensivepricecad", comprehensiveCA);
                    }



                    if (policy.Attributes.Contains("kp_comprehensivepriceus"))
                    {
                        policy.Attributes["kp_comprehensivepriceus"] = comprehensiveUS;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_comprehensivepriceus", comprehensiveUS);
                    }




                    if (policy.Attributes.Contains("kp_liabilitypricecad"))
                    {
                        policy.Attributes["kp_liabilitypricecad"] = liabilityCA;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_liabilitypricecad", liabilityCA);
                    }





                    if (policy.Attributes.Contains("kp_liabilitypriceus"))
                    {
                        policy.Attributes["kp_liabilitypriceus"] = liabilityUS;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_liabilitypriceus", liabilityUS);
                    }

                    if (policy.Attributes.Contains("kp_protectedpricecad"))
                    {
                        policy.Attributes["kp_protectedpricecad"] = personalCA;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_protectedpricecad", personalCA);
                    }

                    if (policy.Attributes.Contains("kp_protectedpriceus"))
                    {
                        policy.Attributes["kp_protectedpriceus"] = personalUS;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_protectedpriceus", personalUS);
                    }





                    if (policy.Attributes.Contains("kp_uninsuredpricecad"))
                    {
                        policy.Attributes["kp_uninsuredpricecad"] = uninsuredCA;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_uninsuredpricecad", uninsuredCA);
                    }

                    if (policy.Attributes.Contains("kp_uninsuredpriceus"))
                    {
                        policy.Attributes["kp_uninsuredpriceus"] = uninsuredUS;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_uninsuredpriceus", uninsuredUS);
                    }

                    Money premiumZero = new Money(0);
                    if (policy.Attributes.Contains("kp_premium"))
                    {
                        policy.Attributes["kp_premium"] = premiumZero;
                    }
                    else
                    {
                        policy.Attributes.Add("kp_premium", premiumZero);
                    }

                    tracingService.Trace("begin server update");
                    tracingService.Trace(((Money)policy.Attributes["kp_premium"]).Value.ToString());
                    //policy.Attributes.Add("kp_premium", 0);
                    service.Update(policy);
                    tracingService.Trace("finished server update");
                    tracingService.Trace(((Money)policy.Attributes["kp_premium"]).Value.ToString());


                }//try block end

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
