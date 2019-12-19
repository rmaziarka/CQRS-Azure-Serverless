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
        public async Task Normal_Orders_Must_Be_In_Normal_Orders_Container()
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
