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
                LibraryId = Guid.Parse("a5b30ee9-e229-49d8-aab8-ba4463e2fe14").ToString(),
                BrandId = Guid.Parse("9f5da8f5-24da-40d4-93f6-9ca980ff9b54").ToString(),
                CopyNumbers = new List<string>(){ "123-123-123", "456-456-456", "789-789-789" }
            });

            httpResponse.EnsureSuccessStatusCode();
        }
    }
 }
