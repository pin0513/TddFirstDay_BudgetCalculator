using System;
using System.Collections.Generic;

namespace TennisGame01
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
                return GetBudgetPerMonth(budgetList, start, end);
            }
            else
            {
                var firstMonthEndDay = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                var lastMonthStartDay = new DateTime(end.Year, end.Month, 1);
                var amount = GetBudgetPerMonth(budgetList,start, firstMonthEndDay) + GetBudgetPerMonth(budgetList, lastMonthStartDay, end);
                var targetStart = start.AddMonths(1);
                var targetEnd = end.AddMonths(-1);
                while (targetStart < targetEnd)
                {
                    amount += budgetList.ContainsKey(targetStart.ToString("yyyyMM")) ? budgetList[targetStart.ToString("yyyyMM")] : 0;
                    targetStart = targetStart.AddMonths(1);

                }

                return amount;
            }

            if (!budgetList.ContainsKey(start.ToString("yyyyMM")))
            {
                return 0;
            }

            throw new ArgumentException("bad...");
        }

        private decimal GetBudgetPerMonth(Dictionary<string, decimal> budgetList, DateTime start, DateTime end)
        {
            if (budgetList.ContainsKey(start.ToString("yyyyMM")))
            {
                int dayDiffs = (end - start).Days;
                var amount = budgetList[start.ToString("yyyyMM")];

                var daysOfMonth = DateTime.DaysInMonth(start.Year, start.Month);
                return (amount / daysOfMonth) * (dayDiffs + 1);
            }
            else
                return 0;
        }
    }
}