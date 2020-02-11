using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LibraryCorp.Tests.Integration
{
    [Collection(nameof(TestCollection))]
    public class CreateCopiesTests
    {
        public CreateCopiesTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly TestFixture _fixture;

        [Fact]
        public async Task AddCopies_creates_book_copies()
        {
            var httpResponse = await _fixture.Client.PostJsonAsync("api/copies", new AddCopies
            {
                LibraryId = Guid.Parse("2e4abb3e-fce5-40be-bce5-e6ca7a66aabe").ToString(),
                BrandId = Guid.Parse("72595fb4-dd7a-4bfe-8ee9-1f5125feb07f").ToString(),
                CopyNumbers = new List<string>(){ "123-123-123", "456-456-456", "789-789-789" }
            });

            httpResponse.EnsureSuccessStatusCode();
        }
    }
 }
