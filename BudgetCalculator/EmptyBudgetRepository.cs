using System.Collections.Generic;

namespace BudgetCalculator
{
    public class EmptyBudgetRepository : IBudgetRepo
    {
        public IList<Budget> GetAll()
        {
            return new List<Budget>();
        }
    }
}