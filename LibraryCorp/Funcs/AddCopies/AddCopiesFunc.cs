using System;
using System.Threading.Tasks;
using LibraryCorp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace LibraryCorp.Funcs.AddCopies
{
    public static class AddCopiesFunc
    {
        [FunctionName("AddCopies")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "copies")] AddCopies command,
            ILogger log)
        {
            try
            {
                var repo = new CosmosRepo(command.LibraryId);
                repo.StartTransaction();
                
                foreach (var copyNumber in command.CopyNumbers)
                {
                    var copy = new Copy(command.LibraryId, command.BrandId, copyNumber);
                    repo.Create(copy);
                }

                await repo.ExecuteAsync();

                return new OkResult();

            }
            catch (Exception e)
            {
                log.LogError(e, e.StackTrace);

                throw;
            }
        }
    }
}
