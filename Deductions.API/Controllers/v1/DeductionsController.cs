using System.Net.Mime;
using Deductions.Domain.Models;
using Deductions.Services;
using Microsoft.AspNetCore.Authorization;
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

        public DeductionsController(ILogger<DeductionsController> logger, IEmployeeRepository employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get the employee's total paycheck deduction amount with dependennts
        /// </summary>
        /// <param name="getEmployeeDeduction"></param>
        /// <returns></returns>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public decimal GetEmployeePaycheckDeduction([FromBody] Employee getEmployeeDeduction)
        {
            var result = _employeeService.GetEmployeeTotalCostPerPaycheck(getEmployeeDeduction);

            return 0.00m;
        }
    }
}