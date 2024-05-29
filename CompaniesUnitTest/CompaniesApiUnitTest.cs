using Flurl.Http;
using OpenapiCompanies.Companies;
using System.Net;
using System.Text.Json;

namespace CompaniesUnitTest
{
    [Collection(nameof(TestCollection))]
    public class CompaniesApiUnitTest : BaseTest
    {
        private readonly IFlurlClient _client;


        public CompaniesApiUnitTest(TestServerFixture testServerFixture) : base(testServerFixture)
        {
            _client = flurlClient;
        }

        [Fact]
        public async Task TestCompany1()
        {
            IFlurlResponse result = await _client.Request("v1", $"companies/1").GetAsync();
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task TestCompany2()
        {
            IFlurlResponse result = await _client.Request("v1", $"companies/2").GetAsync();
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task TestCompany1Data()
        {
            var result = await _client.Request("v1", $"companies/1").GetJsonAsync<Company>();
            Assert.Equal(1, result?.Id);
            Assert.Equal("OpenPolytechnic", result?.Name);
            Assert.Equal("..is awesome", result?.Description);
        }

        [Fact]
        public async Task TestCompany2Data()
        {
            var result = await _client.Request("v1", $"companies/2").GetJsonAsync<Company>();
            Assert.Equal(2, result?.Id);
            Assert.Equal("Other", result?.Name);
            Assert.Equal("....is not", result?.Description);
        }

        [Fact]
        public async Task TestCompany9Data()
        {
            var client = _testServerFixture.CreateClient();
            var response = await client.GetAsync("/v1/companies/9");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<ErrorMessage>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(result);
            Assert.Equal("Not Found", result?.Error);
            Assert.Equal("Can not found the company 9", result?.Error_description);

        }

        

    }
}