using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace kalimataInsurance
{
    public class ClaimPostCreate : IPlugin
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
                Entity claim = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    string fetchDistType = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='kp_configurations'>
                        <attribute name='kp_configurationsid' />
                        <attribute name='kp_configurations' />
                        <attribute name='createdon' />
                        <attribute name='kp_assignclaimby' />
                        <attribute name='kp_value' />
                        <order attribute='kp_configurations' descending='false' />
                        <filter type='and'>
                          <condition attribute='kp_configurationsid' operator='eq' uiname='DISTRIBUTION-TYPE' uitype='kp_configurations' value='{0557ED41-E8A3-E911-A990-000D3A40543B}' />
                          <condition attribute='kp_value' operator='eq' value='true' />
                        </filter>
                      </entity>
                    </fetch>";

                    EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchDistType));
                    Guid distGuid = new Guid("0557ED41-E8A3-E911-A990-000D3A40543B");
                    Entity resultOne = service.Retrieve("kp_configurations", distGuid, new ColumnSet("kp_assignclaimby"));

                    if (resultOne.FormattedValues["kp_assignclaimby"].ToString() == "Cyclic")
                    {
                        /*
                         * Assigning based on cyclic
                         */

                        // Plug-in business logic goes here.  
                 
                        Guid cyclicID = new Guid("{0C243408-7DA3-E911-A990-000D3A40543B}");
                        Entity cyclicInfo = service.Retrieve("kp_cyclicprocess", cyclicID, new ColumnSet("kp_currentindex", "kp_user"));

                        //Entity currentClaim = service.Retrieve("kp_claim", claim.Id, new ColumnSet("ownerid"));

                        Guid currentUserID = ((EntityReference)cyclicInfo.Attributes["kp_user"]).Id;

                        claim.Attributes["ownerid"] = new EntityReference("systemuser", currentUserID);

                        //Entity claimToUpdateCyclic = service.Retrieve("kp_claim", claim.Id, new ColumnSet(true));
                        //claimToUpdateCyclic["ownerid"] = new EntityReference("systemuser", currentUserID);
                        //service.Update(claimToUpdateCyclic);



                        tracingService.Trace(currentUserID.ToString());



                        tracingService.Trace(claim.Attributes["ownerid"].ToString());
                        //tracingService.Trace(currentClaim.Attributes["ownerid"].ToString());
                        //claim.Attributes["ownerid"] = currentUserID;
                        service.Update(claim);

                    }
                    else if (resultOne.FormattedValues["kp_assignclaimby"].ToString() == "Lowest Workload")
                    {

                        /*
                         Assigning based on lowest load
                         */

                        string fetchClaimsTeam = @" <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>  
                         <entity name='systemuser'>    
		                    <attribute name='systemuserid' groupby='true' alias='userID'/> 
		                    <attribute name='fullname' groupby='true' alias='name'/> 
		                    <link-entity name='teammembership' from='systemuserid' to='systemuserid' visible='false' intersect='true'>
                                <link-entity name='team' from='teamid' to='teamid' alias='ac'>
                                  <filter type='and'>
				                    <condition attribute='teamid' operator='eq' uiname='Claim Workers' uitype='team' value='{09450EA9-34A3-E911-A990-000D3A40543B}' />
                                  </filter>
		                    </link-entity>
		                    </link-entity>
                            <link-entity name='kp_claim' from='owninguser' to='systemuserid'> 		
                               <attribute name='kp_claimid' aggregate='count' alias='count'/> 
		                        <filter>
				                    <condition attribute='statecode' operator='eq' value='active'/>
			                    </filter>
                            </link-entity> 	
                         </entity>   
                       </fetch>";

                        EntityCollection resultClaimTeam = service.RetrieveMultiple(new FetchExpression(fetchClaimsTeam));
                        int indexLowestLoad = -1;
                        int currentLowestCount = 9999999;
                        //int loopIndex = 0;
                        for (int i = 0; i < resultClaimTeam.Entities.Count(); i++)
                        {
                            string aliasString = ((AliasedValue)resultClaimTeam.Entities[i].Attributes["count"]).Value.ToString();
                            int userCountNum = int.Parse(aliasString);
                            if (userCountNum < currentLowestCount)
                            {
                                currentLowestCount = userCountNum;
                                indexLowestLoad = i;
                            }
                        }

                        string guidStringLow = ((AliasedValue)resultClaimTeam.Entities[indexLowestLoad].Attributes["userID"]).Value.ToString();

                        Guid guidOfLowUser = new Guid(guidStringLow);
                        Entity claimToUpdate = service.Retrieve("kp_claim", claim.Id, new ColumnSet(true));
                        claimToUpdate["ownerid"] = new EntityReference("systemuser", guidOfLowUser);
                        service.Update(claimToUpdate);

                    }

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