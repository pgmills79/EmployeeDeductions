using Deductions.Domain;

namespace Deductions.Services
{
    public abstract class BaseRepository
    {

        private readonly IUtility _utilityService;
        protected BaseRepository(IUtility utilityService)
        {
            _utilityService = utilityService;
        }
        
        protected decimal GetDeductionAmount(string name, decimal discountAmount, decimal regularDeduction)
        {
            if (string.IsNullOrEmpty(name)) return 0;
            return _utilityService.DoesNameStartWithLetter(name, Constants.ApplyDiscountLetter) ? discountAmount : regularDeduction;
        }
        
    }
}