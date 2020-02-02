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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "reserveBook")] ReserveBook command,
            ILogger log)
        {
            var container = CosmosClientFactory.GetLibrariesContainer();

            var copyToReserve = container.GetFreeCopy(command.BrandId);
            
            var batch = container.CreateTransactionalBatch(new PartitionKey(command.LibraryId));
            copyToReserve.Block();
            batch.UpdateItem(copyToReserve); // etag check

            var reservation = new Reservation(copyToReserve.Id, command.ReaderId);
            batch.CreateItem(reservation);
            await batch.ExecuteAsync();

            return (ActionResult) new OkObjectResult($"Hello, {command.LibraryId}");
        }
    }
}
