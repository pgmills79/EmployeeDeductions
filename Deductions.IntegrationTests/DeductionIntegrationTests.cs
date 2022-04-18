using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Deductions.API.Validation;
using Deductions.Domain;
using Deductions.Domain.Models;
using Deductions.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Deductions.IntegrationTests
{
    public class DeductionIntegrationTests : IClassFixture<ApiWebApplicationFactory>
    {
        private const string Endpoint = "https://localhost:5001/api/v1/deductions";
        private const string MediaType = "application/json";
        private readonly HttpClient _client;
        private readonly IEmployeeRepository _employeeService;
        private readonly ISpouseRepository _spouseService;
        private readonly IDependentRepository _dependentService;

        public DeductionIntegrationTests(ApiWebApplicationFactory fixture)
        {
            _client = fixture.CreateClient();
            _employeeService = fixture.Services.GetService<IEmployeeRepository>();
            _spouseService = fixture.Services.GetService<ISpouseRepository>();
            _dependentService = fixture.Services.GetService<IDependentRepository>();
        }
        
        [Fact]
        public async void GetEmployeeNoSpousePaycheckDeduction_ShouldBe_Successes()
        {
            // Arrange
            const string payload = "{\"Employee\":\"Bob Jones\",\"Dependents\":[{\"Dependent\":\"Tom Kelly\"}]}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var response = await _client.PostAsync(Endpoint, c);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async void GetEmployee_With_Spouse_No_Dependents_ShouldBe_Successes()
        {
            // Arrange
            const string payload = "{\"Employee\":\"Bob Jones\",\"Spouse\":\"Tom Kelly\"}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var response = await _client.PostAsync(Endpoint, c);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async void GetEmployee_With_Spouse_AND_A_Dependent_ShouldBe_Successes()
        {
            // Arrange
            const string payload = "{\"Employee\":\"Bob Jones\",\"Spouse\":\"Sally May\",\"Dependents\":[{\"Dependent\":\"Tom Kelly\"}]}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var response = await _client.PostAsync(Endpoint, c);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async void GetEmployeeTotalCostPerPaycheck_Spouse_Only_No_Discounts_Should_Return_Correct_Values()
        {
            // Arrange
            const string employeeName = "8aron Kelly";
            const string spouseName = "9aron Kelly";
            var payload = $@"{{""Employee"":""{employeeName}"",""Spouse"": ""{spouseName}""}}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);
            var totalDeductionAmount = _employeeService.GetEmployeeDeductionAmount(employeeName) + _spouseService.GetSpouseDeductionAmount(spouseName);
            var paycheckAmount = Constants.MaximumDeductionAmount - totalDeductionAmount;

            //Act
            var response = await _client.PostAsync(Endpoint, c);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<DeductionResult>(await response.Content.ReadAsStringAsync());

            result.Name.Should().Be(employeeName);
            result.Spouse.Should().Be(spouseName);
            result.TotalDeductionAmount.Should().Be($"{totalDeductionAmount:C}");
            result.TotalAmountOfDiscount.Should().Be("$0.00");
            result.NumberOfDependents.Should().Be(0);
            result.PaycheckAmount.Should().Be($"{paycheckAmount:C}");

        }
        
        [Fact]
        public async void GetEmployeeTotalCostPerPaycheck_Spouse_Only_With_One_Discount_Should_Return_Correct_Amounts()
        {
            // Arrange
            const string employeeName = "9aron Kelly";
            var spouseName = $"{Constants.ApplyDiscountLetter}aron Kelly";
            var payload = $@"{{""Employee"":""{employeeName}"",""Spouse"": ""{spouseName}""}}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);
            var spouseDiscountAmount = _spouseService.GetSpouseDiscountAmount();
            var totalDeductionAmount = _employeeService.GetEmployeeDeductionAmount(employeeName) + _spouseService.GetSpouseDiscountAmount();
            var paycheckAmount = Constants.MaximumDeductionAmount - totalDeductionAmount;

            //Act
            var response = await _client.PostAsync(Endpoint, c);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<DeductionResult>(await response.Content.ReadAsStringAsync());

            result.Name.Should().Be(employeeName);
            result.Spouse.Should().Be(spouseName);
            result.TotalDeductionAmount.Should().Be($"{totalDeductionAmount:C}");
            result.TotalAmountOfDiscount.Should().Be($"{spouseDiscountAmount:C}");
            result.NumberOfDependents.Should().Be(0);
            result.PaycheckAmount.Should().Be($"{paycheckAmount:C}");
        }
        
        [Fact]
        public async void GetEmployeeTotalCostPerPaycheck_NoSpouse_One_Dependent_NoDiscount_Should_Return_Correct_Amounts()
        {
            // Arrange
            const string employeeName = "9aron Kelly";
            var dependentName = "8 Kelly";
            var payload = $@"{{""Employee"":""{employeeName}"",""Dependents"": [{{""Dependent"": ""{dependentName}""}}]}}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);
            var dependentDeductionAmount =
                Convert.ToDecimal(Constants.DependentAnnualCost / Constants.NumberOfPaychecks);
            var totalDeductionAmount = _employeeService.GetEmployeeDeductionAmount(employeeName) + dependentDeductionAmount;
            var paycheckAmount = Constants.MaximumDeductionAmount - totalDeductionAmount;

            //Act
            var response = await _client.PostAsync(Endpoint, c);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<DeductionResult>(await response.Content.ReadAsStringAsync());

            result.Name.Should().Be(employeeName);
            result.Spouse.Should().BeNullOrEmpty();
            result.TotalDeductionAmount.Should().Be($"{totalDeductionAmount:C}");
            result.TotalAmountOfDiscount.Should().Be("$0.00");
            result.NumberOfDependents.Should().Be(1);
            result.PaycheckAmount.Should().Be($"{paycheckAmount:C}");
        }
        
        [Fact]
        public async void Empty_Employee_Name_Should_Return_Validation_Error()
        {
            // Arrange
            const string payload = @"{""Employee"":""""}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var response = await _client.PostAsync(Endpoint, c);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var validationResult =await response.Content.ReadAsStringAsync();
            validationResult.Should().Contain(DeductionValidate.EmptyEmployeeName);

        }
        
        [Fact]
        public async void Empty_Provided_Spouse_Name_Should_Return_Validation_Error()
        {
            // Arrange
            const string payload = @"{""Employee"":""Bob Jones"", ""Spouse"":""""}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var response = await _client.PostAsync(Endpoint, c);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var validationResult =await response.Content.ReadAsStringAsync();
            validationResult.Should().Contain(DeductionValidate.EmptySpouseName);

        }
        
        [Fact]
        public async void Empty_Provided_Dependent_Name_Should_Return_Validation_Error()
        {
            // Arrange
            const string employeeName = "Baron Kelly";
            var dependentName = string.Empty;
            var payload = $@"{{""Employee"":""{employeeName}"",""Dependents"": [{{""Dependent"": ""{dependentName}""}}]}}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, MediaType);

            //Act
            var response = await _client.PostAsync(Endpoint, c);
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var validationResult = await response.Content.ReadAsStringAsync();
            validationResult.Should().Contain(DeductionValidate.EmptyDependentName);

        }
    }
}