using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace LibraryCorp
{
    public static class BorrowBookFunc
    {
        [FunctionName("BorrowBook")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "borrowBook")] BorrowBook command,
            ILogger log)
        {
            try
            {
                var repo = new CosmosRepo(command.LibraryId);
                repo.StartTransaction();

                var reservation = await repo.GetReservation(command.ReaderId, command.ReservationId);
                reservation.Borrow();
                
                var borrow = new Borrow(command.ReaderId, reservation.CopyId);
                repo.Create(borrow);
                await repo.ExecuteAsync();

                return new OkObjectResult(new { borrowId = borrow.Id});

            }
            catch (Exception e)
            {
                log.LogError(e, e.StackTrace);

                throw;
            }
        }
    }
}