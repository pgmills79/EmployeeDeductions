using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Deductions.IntegrationTests
{
    public class DeductionTests : IClassFixture<ApiWebApplicationFactory>
    {
        private const string Endpoint = "https://localhost:5001/api/v1/deductions";
        private const string MediaType = "application/json";
        private readonly HttpClient _client;
        
        public DeductionTests(ApiWebApplicationFactory fixture)
        {
            _client = fixture.CreateClient();
        }
        
        [Fact]
        public async void GetEmployeePaycheckDeduction_ShouldBe_Successes()
        {
            // Arrange
            const string payload = "{\"Employee Name\":\"Bob Jones\",\"dependents\":[{\"name\":\"Tom Kelly\"}]}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var result = await _client.PostAsync(Endpoint, c);
            
            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}