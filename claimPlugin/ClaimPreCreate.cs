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
    public class ClaimPreCreate : IPlugin
    {
        private void cyclicProcess(IOrganizationService service, EntityCollection teamUsers, ITracingService tracingService)
        {
            Guid cyclicID = new Guid("{0C243408-7DA3-E911-A990-000D3A40543B}");
            Entity cyclicInfo = service.Retrieve("kp_cyclicprocess", cyclicID, new ColumnSet("kp_currentindex", "kp_user"));
            Guid currentUserID = ((EntityReference)cyclicInfo.Attributes["kp_user"]).Id;
            int currentIndex = (int)cyclicInfo.Attributes["kp_currentindex"];

            //Move to next manager
            if (currentUserID == teamUsers.Entities.ElementAt(currentIndex).Id)
            {
                tracingService.Trace("Sucess");
                currentIndex++;
                if (currentIndex >= teamUsers.Entities.Count)
                {
                    currentIndex = 0;
                }
            }
            else
            {
                tracingService.Trace("Fail");
            }
            //Update
            cyclicInfo.Attributes["kp_currentindex"] = currentIndex;
            cyclicInfo.Attributes["kp_user"] = teamUsers.Entities.ElementAt(currentIndex).ToEntityReference();
            service.Update(cyclicInfo);
        }

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
                    // Plug-in business logic goes here.  
                    string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                  <entity name='systemuser'>
                    <attribute name='fullname' />
                    <attribute name='businessunitid' />
                    <attribute name='title' />
                    <attribute name='address1_telephone1' />
                    <attribute name='positionid' />
                    <attribute name='systemuserid' />
                    <attribute name='createdon' />
                    <order attribute='createdon' descending='false' />
                    <order attribute='fullname' descending='false' />
                    <link-entity name='teammembership' from='systemuserid' to='systemuserid' visible='false' intersect='true'>
                      <link-entity name='team' from='teamid' to='teamid' alias='ac'>
                        <filter type='and'>
                          <condition attribute='teamid' operator='eq' uiname='Claim Workers' uitype='team' value='{09450EA9-34A3-E911-A990-000D3A40543B}' />
                        </filter>
                      </link-entity>
                    </link-entity>
                  </entity>
                </fetch>";


                    EntityCollection teamUsers = service.RetrieveMultiple(new FetchExpression(fetch));
                    tracingService.Trace(teamUsers.Entities.Count.ToString());

                    //string fetchUserWithLowClaims = 
                    //EntityCollection lowClaimsUsers = service.RetrieveMultiple(new FetchExpression(fetchUserWithLowClaims));
                    //Entity lowest = lowClaimsUsers.Entities.First();
                    //tracingService.Trace(lowest.Attributes.First().ToString());

                    cyclicProcess(service, teamUsers, tracingService);

                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin2.", ex);
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