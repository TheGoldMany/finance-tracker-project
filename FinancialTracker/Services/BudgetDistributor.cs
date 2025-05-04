using System.Collections.Generic;
using System.Linq;
using FinancialTracker.Models;

namespace FinancialTracker.Services
{
    public class BudgetDistributor
    {
        private readonly CategorySettings[] _categorySettings;

        public BudgetDistributor()
        {
            _categorySettings = CategorySettings.DefaultSettings;
        }

        public BudgetDistributor(CategorySettings[] customSettings)
        {
            _categorySettings = customSettings;
        }

        public Dictionary<ExpenseCategory, decimal> DistributeBudget(decimal totalAmount)
        {
            var result = new Dictionary<ExpenseCategory, decimal>();
            var remainingAmount = totalAmount;

            // Prioritás szerint rendezzük a kategóriákat
            var orderedCategories = _categorySettings.OrderBy(c => c.Priority).ToList();

            // Minden kategóriához kiszámoljuk a maximális összeget
            foreach (var category in orderedCategories)
            {
                decimal maxAmount = totalAmount * (category.MaxPercentage / 100);

                // Ha van még elegendő összeg, akkor a maximumot adjuk
                if (remainingAmount >= maxAmount)
                {
                    result[category.Category] = maxAmount;
                    remainingAmount -= maxAmount;
                }
                else
                {
                    // Ha nincs elég, akkor a maradékot adjuk
                    result[category.Category] = remainingAmount;
                    remainingAmount = 0;
                    break;
                }
            }

            // Ha még mindig van maradék, adjuk az utolsó (Egyéb) kategóriához
            if (remainingAmount > 0)
            {
                var lastCategory = ExpenseCategory.Egyéb;
                if (result.ContainsKey(lastCategory))
                {
                    result[lastCategory] += remainingAmount;
                }
                else
                {
                    result[lastCategory] = remainingAmount;
                }
            }

            return result;
        }
    }
}