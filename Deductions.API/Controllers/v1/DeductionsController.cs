using System.Net.Mime;
using Deductions.Domain.Models;
using Deductions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deductions.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DeductionsController : ControllerBase
    {

        private readonly ILogger<DeductionsController> _logger;
        private readonly IEmployeeRepository _employeeService;
        private readonly IDependentRepository _dependentService;


        public DeductionsController(ILogger<DeductionsController> logger, IEmployeeRepository employeeService, IDependentRepository dependentService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _dependentService = dependentService;
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
            
            //get employee deduction amount
            var employeeDeductionAmount = _employeeService.GetEmployeeDeductionAmount(employeeEntity.Name);
            
            //get dependents deduction amount
            var dependentDeductionAmount =
                _dependentService.GetDependentsPaycheckDeductionAmount(employeeEntity.Dependents);

            var totalDeductionAmount = _employeeService.GetEmployeeTotalCostPerPaycheck(employeeDeductionAmount, dependentDeductionAmount);

            return new JsonResult(new DeductionResult
            {
                Name = employeeEntity.Name,
                TotalDeductionAmount = totalDeductionAmount,
                NumberOfDependents = employeeEntity.Dependents.Count
            });
        }
    }
}