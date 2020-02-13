using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LibraryCorp.Tests.Integration
{
    [Collection(nameof(TestCollection))]
    public class ReserveBookTests
    {
        public ReserveBookTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly TestFixture _fixture;

        [Fact]
        public async Task ReserveBook_reserves_first_free_copy()
        {
            var httpResponse = await _fixture.Client.PostJsonAsync("api/reserveBook", new ReserveBook()
            {
                LibraryId = Guid.Parse("a5b30ee9-e229-49d8-aab8-ba4463e2fe14").ToString(),
                BrandId = Guid.Parse("9f5da8f5-24da-40d4-93f6-9ca980ff9b54").ToString(),
                ReaderId = Guid.Parse("9638c3ed-4d8b-4c04-b1e5-2d7276886280").ToString()
            });

            httpResponse.EnsureSuccessStatusCode();
        }
    }
 }
