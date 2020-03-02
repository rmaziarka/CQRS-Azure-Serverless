using System;
using System.Threading.Tasks;
using LibraryCorp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace LibraryCorp.Funcs.BorrowBook
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

                var (reservation, copy) = await repo.GetReservationToBorrow(
                    new ReaderId(command.ReaderId), 
                    new ReservationId(command.ReservationId));
                var borrow = reservation.Borrow();
                copy.ChangeOwner(new OwnerId(borrow.BorrowId));

                repo.Create(borrow);
                await repo.ExecuteAsync();

                return new OkObjectResult(new { borrowId = borrow.BorrowId });

            }
            catch (Exception e)
            {
                log.LogError(e, e.StackTrace);

                throw;
            }
        }
    }
}
