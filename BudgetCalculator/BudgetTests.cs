using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BudgetCalculator
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
            var emptyBudgetCalculator = new BudgetCalculator(new EmptyBudgetRepository());
            var budget = emptyBudgetCalculator.TotalAmount(DateTime.MinValue, DateTime.MinValue);
            Assert.AreEqual(0, budget);
        }

        BudgetCalculator budgetCalculator = new BudgetCalculator(new TestDataBudgetRepository(null));
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongStartEnd()
        {
            var budget = budgetCalculator.TotalAmount(new DateTime(2018, 12, 31), new DateTime(2017, 12, 31));
        }


        private void AmountShouldBe(int expected, DateTime start, DateTime end)
        {
            var budget = budgetCalculator.TotalAmount(start, end);
            Assert.AreEqual(expected, budget);
        }

        [TestMethod]
        public void AllOutOfRangeBudget()
        {
            var budget = budgetCalculator.TotalAmount(new DateTime(2017, 12, 31), new DateTime(2017, 12, 31));
            Assert.AreEqual(0, budget);
        }

        [TestMethod]
        public void SingleMonthHaveBudget()
        {
            AmountShouldBe(310, new DateTime(2018, 01, 01), new DateTime(2018, 01, 31));
        }

        [TestMethod]
        public void SingleMonthDaysHaveBudget()
        {
            AmountShouldBe(20, new DateTime(2018, 01, 01), new DateTime(2018, 01, 02));
        }


        [TestMethod]
        public void CrossMonthHaveBudget()
        {
            AmountShouldBe(50, new DateTime(2018, 03, 31), new DateTime(2018, 04, 01));
        }

        [TestMethod]
        public void CrossMoreMonthHaveBudget()
        {
            AmountShouldBe(720, new DateTime(2018, 1, 31), new DateTime(2018, 4, 3));
        }

        [TestMethod]
        public void CrossYearHaveBudget()
        {
            var budget = budgetCalculator.TotalAmount(new DateTime(2017, 08, 31), new DateTime(2018, 06, 01));
            Assert.AreEqual(1830, budget);
        }
        [TestMethod]
        public void CrossmMonthNotBudget()
        {
            var budget = budgetCalculator.TotalAmount(new DateTime(2018, 2, 15), new DateTime(2018, 03, 15));
            Assert.AreEqual(300, budget);
        }
        [TestMethod]
        public void OutOfRangBudget()
        {
            var budget = budgetCalculator.TotalAmount(new DateTime(2019, 1, 1), new DateTime(2019, 12, 31));
            Assert.AreEqual(0, budget);
        }


    }

    public interface IBudgetRepo
    {
        Dictionary<string, Budget> GetAll();
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount{get;set;}
    }

    public class TestDataBudgetRepository : IBudgetRepo
    {
        private IList<Budget> _data;

        public TestDataBudgetRepository(IList<Budget> data)
        {
            _data = data;
        }

        public Dictionary<string, Budget> GetAll()
        {
            return new Dictionary<string, Budget>()
            {
                { "201801", new Budget(){Amount = 310, YearMonth = "201801"}},
                { "201803", new Budget(){Amount = 620, YearMonth = "201803"}},
                { "201804", new Budget(){Amount = 900, YearMonth = "201804"}},
            };
        }
    }

    public class EmptyBudgetRepository : IBudgetRepo
    {
        public EmptyBudgetRepository()
        {
        }

        public Dictionary<string, Budget> GetAll()
        {
            return new Dictionary<string, Budget>();
        }
    }
}