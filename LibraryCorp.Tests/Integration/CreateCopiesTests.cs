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
                LibraryId = Guid.NewGuid().ToString(),
                BrandId = Guid.NewGuid().ToString(),
                CopyNumbers = new List<string>(){ "123-123-123", "456-456-456", "789-789-789" }
            });

            httpResponse.EnsureSuccessStatusCode();
        }
    }
 }
