using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BudgetCalculator
{
    /// <summary>
    /// BudgetTests 的摘要說明
    /// </summary>
    [TestClass]
    public class BudgetCalculatorTests
    {
        [TestMethod]
        public void WithNoBudgetData()
        {
            var emptyBudgetCalculator = new BudgetCalculator(new EmptyBudgetRepository());
            var budget = emptyBudgetCalculator.TotalAmount(DateTime.MinValue, DateTime.MinValue);
            Assert.AreEqual(0, budget);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongStartEnd()
        {
            IList<Budget> data = new List<Budget>()
            {
                new Budget() {Amount = 310, YearMonth = "201801"},
                new Budget() {Amount = 620, YearMonth = "201803"},
                new Budget() {Amount = 900, YearMonth = "201804"}
            };

            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository(data));
            budgetCalculator.TotalAmount(new DateTime(2018, 12, 31), new DateTime(2017, 12, 31));
        }

        private void AmountShouldBe(IList<Budget> data, int expected, DateTime start, DateTime end)
        {
            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository(data));
            var budget = budgetCalculator.TotalAmount(start, end);
            Assert.AreEqual(expected, budget);
        }

        [TestMethod]
        public void AllOutOfRangeBudget()
        {
            IList<Budget> data = new List<Budget>()
            {
                new Budget() {Amount = 310, YearMonth = "201801"},
                new Budget() {Amount = 620, YearMonth = "201803"},
                new Budget() {Amount = 900, YearMonth = "201804"}
            };

            AmountShouldBe(data, 0, new DateTime(2017, 12, 31), new DateTime(2017, 12, 31));
        }

        [TestMethod]
        public void SingleMonthHaveBudget()
        {
            IList<Budget> data = new List<Budget>()
            {
                new Budget() {Amount = 310, YearMonth = "201801"},
                new Budget() {Amount = 620, YearMonth = "201803"},
                new Budget() {Amount = 900, YearMonth = "201804"}
            };
            AmountShouldBe(data, 310, new DateTime(2018, 01, 01), new DateTime(2018, 01, 31));
        }

        [TestMethod]
        public void SingleMonthDaysHaveBudget()
        {
            IList<Budget> data = new List<Budget>()
            {
                new Budget() {Amount = 310, YearMonth = "201801"},
                new Budget() {Amount = 620, YearMonth = "201803"},
                new Budget() {Amount = 900, YearMonth = "201804"}
            };

            AmountShouldBe(data, 20, new DateTime(2018, 01, 01), new DateTime(2018, 01, 02));
        }

        [TestMethod]
        public void CrossMonthHaveBudget()
        {
            IList<Budget> data = new List<Budget>()
            {
                new Budget() {Amount = 310, YearMonth = "201801"},
                new Budget() {Amount = 620, YearMonth = "201803"},
                new Budget() {Amount = 900, YearMonth = "201804"}
            };

            AmountShouldBe(data, 50, new DateTime(2018, 03, 31), new DateTime(2018, 04, 01));
        }

        [TestMethod]
        public void CrossMoreMonthHaveBudget()
        {
            IList<Budget> data = new List<Budget>()
            {
                new Budget() {Amount = 310, YearMonth = "201801"},
                new Budget() {Amount = 620, YearMonth = "201803"},
                new Budget() {Amount = 900, YearMonth = "201804"}
            };
            AmountShouldBe(data, 720, new DateTime(2018, 1, 31), new DateTime(2018, 4, 3));
        }

        [TestMethod]
        public void CrossYearHaveBudget()
        {
            IList<Budget> data = new List<Budget>()
            {
                new Budget() {Amount = 310, YearMonth = "201801"},
                new Budget() {Amount = 620, YearMonth = "201803"},
                new Budget() {Amount = 900, YearMonth = "201804"}
            };
            AmountShouldBe(data, 1830, new DateTime(2017, 08, 31), new DateTime(2018, 06, 01));
        }

        [TestMethod]
        public void CrossMonthSomeHaveBudgetSomeNoBudget()
        {
            IList<Budget> data = new List<Budget>()
            {
                new Budget() {Amount = 310, YearMonth = "201801"},
                new Budget() {Amount = 620, YearMonth = "201803"},
                new Budget() {Amount = 900, YearMonth = "201804"}
            };
            AmountShouldBe(data, 300, new DateTime(2018, 2, 15), new DateTime(2018, 03, 15));
        }

        [TestMethod]
        public void OutOfRangBudget()
        {
            IList<Budget> data = new List<Budget>()
            {
                new Budget() {Amount = 310, YearMonth = "201801"},
                new Budget() {Amount = 620, YearMonth = "201803"},
                new Budget() {Amount = 900, YearMonth = "201804"}
            };
            AmountShouldBe(data, 0, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31));
        }
    }
}