using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace RealEstate
{
    public static class EstateCreateFunc
    {
        [FunctionName("EstateCreatedFunc")]
        public static void Run([CosmosDBTrigger(
            databaseName: "RealEstate",
            collectionName: "estates",
            ConnectionStringSetting = "AzureCosmosDBConnection",
            LeaseCollectionName = "leases")]IReadOnlyList<Estate> estates, ILogger log)
        {
            if (estates != null && estates.Count > 0)
            {
                log.LogInformation("Documents modified " + estates[0].Id);
            }
        }
    }
}
