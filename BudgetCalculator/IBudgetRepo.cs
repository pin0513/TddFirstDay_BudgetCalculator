using System.Collections.Generic;

namespace BudgetCalculator
{
    public interface IBudgetRepo
    {
        IList<Budget> GetAll();
    }
}