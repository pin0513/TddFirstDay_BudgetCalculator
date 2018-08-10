using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TennisGame01
{
    /// <summary>
    /// BudgetTests 的摘要說明
    /// </summary>
    [TestClass]
    public class BudgetTests
    {
        public BudgetTests()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        [TestMethod]
        public void WithNoBudgetData()
        {
            var budgetCalculator = new BudgetCalculator(new EmptyBudgetRepository());
            var budget = budgetCalculator.TotalAmount(DateTime.MinValue, DateTime.MinValue);
            Assert.AreEqual(0, budget);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongStartEnd()
        {
            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository());
            var budget = budgetCalculator.TotalAmount(new DateTime(2018, 12, 31), new DateTime(2017, 12, 31));
        }

        [TestMethod]
        public void AllOutOfRangeBudget()
        {
            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository());
            var budget = budgetCalculator.TotalAmount(new DateTime(2017, 12, 31), new DateTime(2017, 12, 31));
            Assert.AreEqual(0, budget);
        }

        [TestMethod]
        public void SingleMonthHaveBudget()
        {
            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository());
            var budget = budgetCalculator.TotalAmount(new DateTime(2018, 01, 01), new DateTime(2018, 01, 31));
            Assert.AreEqual(310, budget);
        }

        [TestMethod]
        public void SingleMonthDaysHaveBudget()
        {
            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository());
            var budget = budgetCalculator.TotalAmount(new DateTime(2018, 01, 01), new DateTime(2018, 01, 02));
            Assert.AreEqual(20, budget);
        }

        [TestMethod]
        public void CrossMonthHaveBudget()
        {
            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository());
            var budget = budgetCalculator.TotalAmount(new DateTime(2018, 03, 31), new DateTime(2018, 04, 01));
            Assert.AreEqual(50, budget);
        }

        [TestMethod]
        public void CrossYearHaveBudget()
        {
            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository());
            var budget = budgetCalculator.TotalAmount(new DateTime(2017, 08, 31), new DateTime(2018, 06, 01));
            Assert.AreEqual(1830, budget);
        }
        [TestMethod]
        public void CrossmMonthNotBudget()
        {
            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository());
            var budget = budgetCalculator.TotalAmount(new DateTime(2018, 2, 15), new DateTime(2018, 03, 15));
            Assert.AreEqual(300, budget);
        }
        [TestMethod]
        public void OutOfRangBudget()
        {
            var budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository());
            var budget = budgetCalculator.TotalAmount(new DateTime(2019, 1, 1), new DateTime(2019, 12, 31));
            Assert.AreEqual(0, budget);
        }
    }

    public interface IBudgetRepo
    {
        Dictionary<string, decimal> GetAll();
    }

    public class TestDataBudgetRepository : IBudgetRepo
    {
        public TestDataBudgetRepository()
        {
        }

        public Dictionary<string, decimal> GetAll()
        {
            return new Dictionary<string, decimal>()
            {
                { "201801", 310},
                { "201803", 620},
                { "201804", 900},
            };
        }
    }

    public class EmptyBudgetRepository : IBudgetRepo
    {
        public EmptyBudgetRepository()
        {
        }

        public Dictionary<string, decimal> GetAll()
        {
            return new Dictionary<string, decimal>();
        }
    }
}