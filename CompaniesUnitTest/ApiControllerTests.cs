using Microsoft.AspNetCore.Mvc.Testing;
using OpenapiCompanies;
using OpenapiCompanies.Companies;
using System.Net;
using System.Text.Json;


namespace CompaniesUnitTest
{
    public class ApiControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ApiControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetCompany2()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/v1/companies/2");

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Company>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(result);
            Assert.Equal(2, result?.Id);
            Assert.Equal("Other", result?.Name);
            Assert.Equal("....is not", result?.Description);
        }

        [Fact]
        public async Task GetCompanyWrongId()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/v1/companies/99");

            // Assert
    
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorMessage>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(result);
            Assert.Equal("Not Found", result?.Error);
            Assert.Equal("Can not found the company 99", result?.Error_description);
        }

        [Fact]
        public async Task GetCompanyStringId()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/v1/companies/AA");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
    }
}
