using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace LibraryCorp
{
    public static class ReserveBookFunc
    {
        [FunctionName("ReserveBook")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "reserveBook")] ReserveBook command,
            ILogger log)
        {
            var repo = new CosmosRepo(command.LibraryId);
            repo.StartTransaction();

            var copyToReserve = await repo.GetFreeCopy(command.BrandId);
            copyToReserve.Block();
            
            var reservation = new Reservation(copyToReserve.Id, command.ReaderId);
            repo.Create(reservation);
            await repo.ExecuteAsync();

            return (ActionResult) new OkObjectResult($"Hello, {command.LibraryId}");
        }
    }
}