using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace LibraryCorp
{
    public static class AddCopiesFunc
    {
        [FunctionName("ChangeFeedListener")]
        public static void Run([CosmosDBTrigger(
            databaseName: Constants.CosmosDBName,
            collectionName: Constants.CosmosCollectionName,
            ConnectionStringSetting = Constants.CosmosConnectionString,
            LeaseCollectionName = "LeaseCollection",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
}
