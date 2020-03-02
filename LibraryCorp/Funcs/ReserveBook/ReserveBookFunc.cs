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

                var copyToReserve = await repo.GetFreeCopy(new BrandId(command.BrandId));
                var reservation = new Reservation(new ReaderId(command.ReaderId), copyToReserve.CopyId);
                copyToReserve.Block(new OwnerId(reservation.ReservationId));
             
                repo.Create(reservation);
                await repo.ExecuteAsync();

                return new OkObjectResult(new { reservationId = reservation.ReservationId });

            }
            catch (Exception e)
            {
                log.LogError(e, e.StackTrace);

                throw;
            }
        }
    }
}
