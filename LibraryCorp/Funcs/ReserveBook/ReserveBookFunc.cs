using System;
using System.Threading.Tasks;
using LibraryCorp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace LibraryCorp.Funcs.ReserveBook
{
    public static class ReturnBookFunc
    {
        [FunctionName("ReserveBook")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "reserveBook")] ReserveBook command,
            ILogger log)
        {
            try
            {
                var repo = new CosmosRepo(command.LibraryId);
                repo.StartTransaction();

                var copyToReserve = await repo.GetFreeCopy(command.BrandId);
                copyToReserve.Block();
                
                var reservation = new Reservation(command.ReaderId, copyToReserve.Id);
                repo.Create(reservation);
                await repo.ExecuteAsync();

                return new OkObjectResult(new { reservationId = reservation.Id });

            }
            catch (Exception e)
            {
                log.LogError(e, e.StackTrace);

                throw;
            }
        }
    }
}
