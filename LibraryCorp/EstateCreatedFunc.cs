using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace LibraryCorp
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
