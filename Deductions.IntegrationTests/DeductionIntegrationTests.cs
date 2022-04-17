using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Deductions.IntegrationTests
{
    public class DeductionIntegrationTests : IClassFixture<ApiWebApplicationFactory>
    {
        private const string Endpoint = "https://localhost:5001/api/v1/deductions";
        private const string MediaType = "application/json";
        private readonly HttpClient _client;
        
        public DeductionIntegrationTests(ApiWebApplicationFactory fixture)
        {
            _client = fixture.CreateClient();
        }
        
        [Fact]
        public async void GetEmployeeNoSpousePaycheckDeduction_ShouldBe_Successes()
        {
            // Arrange
            const string payload = "{\"Employee\":\"Bob Jones\",\"Dependents\":[{\"Dependent\":\"Tom Kelly\"}]}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var result = await _client.PostAsync(Endpoint, c);
            
            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async void GetEmployee_With_Spouse_No_Dependents_ShouldBe_Successes()
        {
            // Arrange
            const string payload = "{\"Employee\":\"Bob Jones\",\"Spouse\":\"Tom Kelly\"}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var result = await _client.PostAsync(Endpoint, c);
            
            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async void GetEmployee_With_Spouse_AND_A_Dependent_ShouldBe_Successes()
        {
            // Arrange
            const string payload = "{\"Employee\":\"Bob Jones\",\"Spouse\":\"Sally May\",\"Dependents\":[{\"Dependent\":\"Tom Kelly\"}]}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var result = await _client.PostAsync(Endpoint, c);
            
            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}