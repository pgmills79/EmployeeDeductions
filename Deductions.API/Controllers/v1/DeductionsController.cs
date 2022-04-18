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
        private readonly ISpouseRepository _spouseService;
        private readonly IDependentRepository _dependentService;

        public DeductionsController(IEmployeeRepository employeeService, IDependentRepository dependentService, ISpouseRepository spouseService)
        {
            _employeeService = employeeService;
            _dependentService = dependentService;
            _spouseService = spouseService;
        }

        /// <summary>
        /// Get the employee's total paycheck deduction amount with dependents
        /// </summary>
        /// <param name="employeeEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult GetEmployeePaycheckDeduction([FromBody] Employee employeeEntity)
        {

            //Grab deduction information
            var totalDeductionAmount = GrabTotalDeductionAmount(employeeEntity);

            //Grab discount information for information purposes
            var employeeTotalDiscountAmount = GrabTotalDiscountAmount(employeeEntity);

            return new JsonResult(new DeductionResult
            {
                Name = employeeEntity.Name,
                Spouse = employeeEntity.Spouse,
                TotalDeductionAmount = $"{totalDeductionAmount:C}",
                TotalAmountOfDiscount = $"{employeeTotalDiscountAmount:C}",
                NumberOfDependents = employeeEntity.Dependents?.Any() == true ? employeeEntity.Dependents.Count : 0,
                PaycheckAmount = $"{Constants.MaximumDeductionAmount - totalDeductionAmount:C}"
            });
        }

        private decimal GrabTotalDiscountAmount(Employee employeeEntity)
        {
            var employeeTotalDiscountAmount = 0.00m;

            //Employee discount amount
            employeeTotalDiscountAmount += _employeeService.DoesEmployeeGetDiscount(employeeEntity.Name)
                ? _employeeService.GetEmployeeDiscountAmount()
                : 0;

            //Spouse discount amount
            employeeTotalDiscountAmount += _spouseService.DoesSpouseGetDiscount(employeeEntity.Spouse)
                ? _spouseService.GetSpouseDiscountAmount()
                : 0;

            employeeTotalDiscountAmount += _dependentService.GetTotalDependentDiscountAmount(employeeEntity.Dependents);
            
            return  employeeTotalDiscountAmount <= Constants.MaximumDeductionAmount ? employeeTotalDiscountAmount : Constants.MaximumDeductionAmount;
        }

        private decimal GrabTotalDeductionAmount(Employee employeeEntity)
        {
            var employeeDeductionAmount = _employeeService.GetEmployeeDeductionAmount(employeeEntity.Name);

            //get employee deduction amount
            var spouseDeductionAmount = !string.IsNullOrEmpty(employeeEntity.Spouse)
                ? _spouseService.GetSpouseDeductionAmount(employeeEntity.Spouse)
                : 0;

            //get dependents deduction amount
            var dependentDeductionAmount =
                _dependentService.GetDependentDeductionAmount(employeeEntity.Dependents);

            var totalDeductionAmount = _employeeService.GetEmployeeTotalCostPerPaycheck(employeeDeductionAmount,
                spouseDeductionAmount,
                dependentDeductionAmount);
            
            return totalDeductionAmount;
        }
    }
}