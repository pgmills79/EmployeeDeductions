using System;
using System.Collections.Generic;
using System.Linq;
using Deductions.Domain;

namespace Deductions.Services
{
    public interface IDependentRepository
    {
        decimal GetDependentDeductionAmount();
        decimal GetDependentDiscountAmount();
        decimal GetDependentsPaycheckDeductionAmount(List<string> dependents);
    }
    
    public class DependentRepository : IDependentRepository
    {
        private const int DependentAnnualCost = 500;
        private readonly IUtility _utilityService;

        public DependentRepository(IUtility utilityService)
        {
            _utilityService = utilityService;
        }

        public decimal GetDependentsPaycheckDeductionAmount(List<string> dependents)
        {
            var deductionAmount = 0.00m;

            //if no dependents found just return zero
            if (dependents?.Any() != true) return 0;

            //add up the dependents amounts
            foreach (var dependent in dependents)
            {
                if (_utilityService.DoesNameStartWithLetter(dependent, IBaseRepository.ApplyDiscountLetter))
                {
                    deductionAmount += GetDependentDiscountAmount();
                }
                else
                {
                    deductionAmount += GetDependentDeductionAmount();
                }
            }
            
            //does name get
            return deductionAmount;
        }
        
        public decimal GetDependentDeductionAmount()
        {
            return Convert.ToDecimal(DependentAnnualCost / IBaseRepository.NumberOfPaychecks);
        }

        public decimal GetDependentDiscountAmount()
        {
            return Convert.ToDecimal( (DependentAnnualCost - IBaseRepository.DiscountPercent * DependentAnnualCost) / IBaseRepository.NumberOfPaychecks);
        }
        
    }
}