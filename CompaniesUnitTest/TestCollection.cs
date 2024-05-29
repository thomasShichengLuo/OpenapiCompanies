using Xunit;

namespace CompaniesUnitTest
{
    [CollectionDefinition(nameof(TestCollection))]
    public class TestCollection : ICollectionFixture<TestServerFixture>
    {

    }
}
