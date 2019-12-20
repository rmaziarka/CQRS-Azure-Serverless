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
        [FunctionName("AddCopies")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "copies")] AddCopies command,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var connectionString =
                Environment.GetEnvironmentVariable("AzureCosmosDBConnection", EnvironmentVariableTarget.Process);

            log.LogInformation("AzureCosmosDBConnection: " + connectionString);

            var container = CosmosClientFactory.GetLibrariesContainer();

            var batch = container.CreateTransactionalBatch(new PartitionKey(command.LibraryId));

            foreach (var copyNumber in command.CopyNumbers)
            {
                var copy = new Copy(command.LibraryId, command.BrandId, copyNumber);
                batch.CreateItem(copy);
            }
            await batch.ExecuteAsync();

            return (ActionResult) new OkObjectResult($"Hello, {command.LibraryId}");
        }
    }
}
