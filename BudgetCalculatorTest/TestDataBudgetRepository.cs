using System.Collections.Generic;

namespace BudgetCalculator
{
    public class TestDataBudgetRepository : IBudgetRepo
    {
        private IList<Budget> _data;

        public TestDataBudgetRepository(IList<Budget> data)
        {
            _data = data;
        }

        public IList<Budget> GetAll()
        {
            return _data;
        }
    }
}