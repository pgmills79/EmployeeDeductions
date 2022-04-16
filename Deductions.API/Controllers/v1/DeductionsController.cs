using System.Linq;
using System.Net.Mime;
using Deductions.Domain;
using Deductions.Domain.Models;
using Deductions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Deductions.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DeductionsController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeService;
        private readonly IDependentRepository _dependentService;
        private readonly IUtility _utilityService;


        public DeductionsController(IEmployeeRepository employeeService, IDependentRepository dependentService, IUtility utilityService)
        {
            _employeeService = employeeService;
            _dependentService = dependentService;
            _utilityService = utilityService;
        }

        /// <summary>
        /// Get the employee's total paycheck deduction amount with dependents
        /// </summary>
        /// <param name="employeeEntity"></param>
        /// <returns></returns>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult GetEmployeePaycheckDeduction([FromBody] Employee employeeEntity)
        {

            //Grab deduction information
            //get employee deduction amount
            var employeeDeductionAmount = _employeeService.GetEmployeeDeductionAmount(employeeEntity.Name);

            //get dependents deduction amount
            var dependentDeductionAmount =
                _dependentService.GetDependentDeductionAmount(employeeEntity.Dependents);
            
            var totalDeductionAmount = _employeeService.GetEmployeeTotalCostPerPaycheck(employeeDeductionAmount, dependentDeductionAmount);
            
            //Grab discount information for information purposes
            var employeeTotalDiscountAmount = 0.00m;
            
            employeeTotalDiscountAmount += _utilityService.DoesNameStartWithLetter(employeeEntity.Name, Constants.ApplyDiscountLetter) 
                ? _employeeService.GetEmployeeDiscountAmount() : 0;
            
            employeeTotalDiscountAmount += _dependentService.GetTotalDependentDiscountAmount(employeeEntity.Dependents);

            return new JsonResult(new DeductionResult
            {
                Name = employeeEntity.Name,
                TotalDeductionAmount = $"{totalDeductionAmount:C}",
                TotalAmountOfDiscount = $"{employeeTotalDiscountAmount:C}",
                NumberOfDependents = employeeEntity.Dependents?.Any() == true ? employeeEntity.Dependents.Count : 0
            });
        }
    }
}