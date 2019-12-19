using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace LibraryCorp
{
    public static class CreateEstateFunc
    {
        [FunctionName("CreateEstate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] CreateEstate command,
            [CosmosDB("RealEstate", "estates", ConnectionStringSetting = "AzureCosmosDBConnection", UseDefaultJsonSerialization = false)] IAsyncCollector<Estate> estates,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var estate = command.ToEstate();

            await estates.AddAsync(estate);

            return (ActionResult) new OkObjectResult($"Hello, {command.CompanyId}");
        }
    }
}
