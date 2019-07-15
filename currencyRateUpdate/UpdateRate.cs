
using System;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using currencyRateUpdate.APIModels;
using System.Web.Script.Serialization;
using System.Net;
using System.Runtime.Serialization.Json;

namespace CurrenyRate
{
    public class UpdateRate : CodeActivity
    {
        private IOrganizationService service;
        //private ITracingService tracingService;
        protected override void Execute(CodeActivityContext context)
        {
            //tracingService = context.GetExtension<ITracingService>();
            //tracingService.Trace("Here");

            // Create Context
            IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();

            // Use the context service to create an instance of IOrganizationService.             
            service = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);

            // Retrive Rate and insert into CRM
            decimal newCADRate = GetUpdateRatesAsync();
            CRMConnection(newCADRate);

        }

        public static decimal GetUpdateRatesAsync()
        {
            // API string and value prep
            string apiConnectionString = "http://data.fixer.io/api/latest?access_key=3edd3e01c73d7f004c08acabe3100d4f&symbols=USD,CAD";
            float USD;
            float CAD;
            Message work;

            // Retrieve JSON as a String
            string messager;
            using (WebClient wc = new WebClient())
            {
                messager = wc.DownloadString(apiConnectionString);
            }

            // Convert String to JSON Object
            using (System.IO.MemoryStream DeserializeStream = new System.IO.MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Message));

                System.IO.StreamWriter writer = new System.IO.StreamWriter(DeserializeStream);
                writer.Write(messager);
                writer.Flush();
                DeserializeStream.Position = 0;

                work = (Message)serializer.ReadObject(DeserializeStream);
            }

            // Calculate New Rate
            USD = float.Parse(work.rates.USD);
            CAD = float.Parse(work.rates.CAD);
            decimal newCADRate = (decimal)(CAD / USD);

            return newCADRate;

        }

        public void CRMConnection(decimal newCADRate)
        {
            // Prep CRM Entity
            Guid iden = Guid.Parse("6f14ded1-1ca2-e911-a9aa-000d3a4025c7");
            ColumnSet target = new ColumnSet("exchangerate");

            // Retrieve Entity
            Entity currencyRecord = service.Retrieve("transactioncurrency", iden, target);

            // Update Entity
            currencyRecord.Attributes["exchangerate"] = newCADRate;
            service.Update(currencyRecord);
        }
    }
}
