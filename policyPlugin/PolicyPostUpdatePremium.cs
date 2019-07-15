using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace kalimataInsurance
{
    public class PolicyPostUpdatePremium : IPlugin
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
                    if (context.Depth > 1) return;
                    Entity currentPolicy = service.Retrieve("kp_policy", entity.Id, new ColumnSet(true));
                    Entity resultDis;
                    //check to see if contact exist
                    if (currentPolicy.Attributes.Contains("kp_policyholder"))
                    {
                        Guid currentContact = ((EntityReference)currentPolicy.Attributes["kp_policyholder"]).Id;

                        resultDis = service.Retrieve("contact", currentContact, new ColumnSet("address1_country"));
                        tracingService.Trace("Country is " + resultDis.Attributes["address1_country"].ToString());
                    }
                    //default to US tax if no contact is found
                    else
                    {
                        resultDis = new Entity();
                        resultDis.Attributes.Add("address1_country", "US");
                    }

                    //set currency base on country
                    Guid CADID = new Guid("6f14ded1-1ca2-e911-a9aa-000d3a4025c7");
                    Guid USDID = new Guid("391b1ea8-0fa2-e911-a9b6-000d3a402766");
                    EntityReference CADRef = new EntityReference("transactioncurrency", CADID);
                    EntityReference USDRef = new EntityReference("transactioncurrency", USDID);
                    if (resultDis.Attributes["address1_country"].ToString().Contains("CA"))
                    {
                        currentPolicy.Attributes["transactioncurrencyid"] = CADRef;
                    }
                    else if (resultDis.Attributes["address1_country"].ToString().Contains("US"))
                    {
                        currentPolicy.Attributes["transactioncurrencyid"] = USDRef;
                    }


                    tracingService.Trace(((Money)currentPolicy.Attributes["kp_premium"]).Value.ToString());
                    //calculate premium
                    if (currentPolicy.Attributes["transactioncurrencyid"].Equals(USDRef))
                    {
                        tracingService.Trace("Calculate premium in USD");
                        //Protected
                        if (entity.Attributes.Contains("kp_protected"))
                        {
                            if (entity.Attributes["kp_protected"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_protectedpriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_protected"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_protectedpriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }

                        //Collision
                        if (entity.Attributes.Contains("kp_collision"))
                        {
                            if (entity.Attributes["kp_collision"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_collisionpriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_collision"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_collisionpriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }

                        //Uninsured
                        if (entity.Attributes.Contains("kp_uninsured"))
                        {
                            if (entity.Attributes["kp_uninsured"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_uninsuredpriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_uninsured"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_uninsuredpriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }
                        //liability
                        if (entity.Attributes.Contains("kp_liability"))
                        {
                            if (entity.Attributes["kp_liability"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_liabilitypriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_liability"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_liabilitypriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }

                        //comprehensive
                        if (entity.Attributes.Contains("kp_comprehensive"))
                        {
                            if (entity.Attributes["kp_comprehensive"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_comprehensivepriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_comprehensive"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_comprehensivepriceus"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }
                        tracingService.Trace("Calculated premium in USD");
                    }
                    else if (currentPolicy.Attributes["transactioncurrencyid"].Equals(CADRef))
                    {
                        tracingService.Trace("Calculate premium in CAD");
                        //Protected
                        if (entity.Attributes.Contains("kp_protected"))
                        {
                            if (entity.Attributes["kp_protected"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_protectedpricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_protected"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_protectedpricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }

                        //Collision
                        if (entity.Attributes.Contains("kp_collision"))
                        {
                            if (entity.Attributes["kp_collision"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_collisionpricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_collision"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_collisionpricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }

                        //Uninsured
                        if (entity.Attributes.Contains("kp_uninsured"))
                        {
                            if (entity.Attributes["kp_uninsured"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_uninsuredpricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_uninsured"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_uninsuredpricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }

                        //liability
                        if (entity.Attributes.Contains("kp_liability"))
                        {
                            if (entity.Attributes["kp_liability"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_liabilitypricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_liability"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_liabilitypricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }

                        //comprehensive
                        if (entity.Attributes.Contains("kp_comprehensive"))
                        {
                            if (entity.Attributes["kp_comprehensive"].Equals(true))
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium + ((decimal)currentPolicy.Attributes["kp_comprehensivepricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                            else if (entity.Attributes["kp_comprehensive"].Equals(false) && ((Money)(currentPolicy.Attributes["kp_premium"])).Value >= 0)
                            {
                                decimal currentPremium = ((Money)currentPolicy.Attributes["kp_premium"]).Value;
                                Money newPremium = new Money(currentPremium - ((decimal)currentPolicy.Attributes["kp_comprehensivepricecad"]));
                                currentPolicy.Attributes["kp_premium"] = newPremium;
                            }
                        }

                        tracingService.Trace("Calculated premium in CAD");
                    }
                    tracingService.Trace(((Money)currentPolicy.Attributes["kp_premium"]).Value.ToString());
                    tracingService.Trace("start update premium");
                    service.Update(currentPolicy);
                    tracingService.Trace("updated premium");
                    tracingService.Trace(((Money)currentPolicy.Attributes["kp_premium"]).Value.ToString());
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
