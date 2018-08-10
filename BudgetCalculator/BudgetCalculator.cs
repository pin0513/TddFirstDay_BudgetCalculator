using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetCalculator
{
    public class BudgetCalculator
    {
        private IBudgetRepo _repo;

        public BudgetCalculator(IBudgetRepo repo)
        {
            _repo = repo;
        }

        public decimal TotalAmount(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException();

            var budgetList = _repo.GetAll();
            if (budgetList.Count == 0)
            {
                return 0;
            }

            if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
            {
                return GetStartMonthAndEndMonthBudgetAmount(budgetList, start, end);
            }
            else
            {
                var firstMonthEndDay = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                var lastMonthStartDay = new DateTime(end.Year, end.Month, 1);
                var amount = GetStartMonthAndEndMonthBudgetAmount(budgetList,start, firstMonthEndDay) + GetStartMonthAndEndMonthBudgetAmount(budgetList, lastMonthStartDay, end);
                var middleStart = new DateTime(start.Year, start.Month, 1).AddMonths(1);
                var middleEnd = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month)).AddMonths(-1);
                while (middleStart < middleEnd)
                {
                    amount += budgetList.Count(a=>a.YearMonth == middleStart.ToString("yyyyMM"))>0 ? budgetList.Single(a=>a.YearMonth == middleStart.ToString("yyyyMM")).Amount : 0;
                    middleStart = middleStart.AddMonths(1);
                }

                return amount;
            }
        }

        private decimal GetStartMonthAndEndMonthBudgetAmount(IList<Budget> budgetList, DateTime start, DateTime end)
        {
            if (budgetList.Count(a=>a.YearMonth == start.ToString("yyyyMM"))>0)
            {
                int dayDiffs = (end - start).Days + 1;
                var amount = budgetList.Single(a=>a.YearMonth == start.ToString("yyyyMM")).Amount;

                var daysOfMonth = DateTime.DaysInMonth(start.Year, start.Month);
                return (amount / (decimal)daysOfMonth) * (dayDiffs);
            }
            else
                return 0;
        }
    }
}