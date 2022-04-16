using System;
using System.Collections.Generic;
using System.Linq;
using Deductions.Domain;
using Deductions.Domain.Models;

namespace Deductions.Services
{
    public interface IDependentRepository
    {
        decimal GetTotalDependentDiscountAmount(List<Dependent> dependents);
        decimal GetDependentDeductionAmount(List<Dependent> dependents);
    }
    
    public class DependentRepository : BaseRepository, IDependentRepository
    {
        private readonly IUtility _utilityService;
        public DependentRepository(IUtility utilityService) : base(utilityService)
        {
            _utilityService = utilityService;
        }

        public decimal GetDependentDeductionAmount(List<Dependent> dependents)
        {
            return dependents?.Any() != true ? 0 : 
                dependents.Sum(dependent => GetDeductionAmount(dependent.Name, GetDependentDiscountAmount(), GetDeductionAmount()));
        }

        public decimal GetTotalDependentDiscountAmount(List<Dependent> dependents)
        {
            if (dependents?.Any() != true) return 0;
            return dependents.Sum(dependent => _utilityService.DoesNameStartWithLetter(dependent.Name, Constants.ApplyDiscountLetter) 
                ? GetDependentDiscountAmount() : 0);
        }
        
        private static decimal GetDeductionAmount()
        {
            return Convert.ToDecimal(Constants.DependentAnnualCost / Constants.NumberOfPaychecks);
        }

        private static decimal GetDependentDiscountAmount()
        {
            return Convert.ToDecimal( (Constants.DependentAnnualCost - Constants.DiscountPercent * Constants.DependentAnnualCost) / Constants.NumberOfPaychecks);
        }
        
    }
}