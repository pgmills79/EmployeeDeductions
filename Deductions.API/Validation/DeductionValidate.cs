using System;
using System.Linq;
using Deductions.Domain.Models;
using FluentValidation;

namespace Deductions.API.Validation
{


    /// <summary>
    /// The validation class for deductions API
    /// </summary>
    public class DeductionValidate : AbstractValidator<Employee>
    {
        public const string EmptyEmployeeName = "Employee name cannot be empty";
        
        public const string EmptySpouseName = "If a spouse is provided you must add a name";
        
        public const string EmptyDependentName = "If a dependent is provided you must add a name";
        
        public DeductionValidate()
        {

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage(EmptyEmployeeName);
            //.MaximumLength(StringFieldMaximumLength).WithMessage(FullNameTooLong);
            
            RuleFor(p => p.Dependents)
                .Must(dependents => dependents == null || dependents.All(x => !string.IsNullOrEmpty(x.Name)))
                .WithMessage(EmptyDependentName);
            
            RuleFor(p => p.Spouse)
                .Must(spouse => spouse == null || !string.IsNullOrEmpty(spouse))
                .WithMessage(EmptySpouseName);
        }
    }
}