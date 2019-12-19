using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LibraryCorp.Tests.Integration
{
    [CollectionDefinition(nameof(TestCollection))]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
    }
}
