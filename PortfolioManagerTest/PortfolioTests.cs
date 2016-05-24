using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PortfolioManager;
using PortfolioManager.Model;
using PortfolioManager.Common.Interfaces.Model;

namespace PortfolioManagerTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PortfolioTests
    {
        public PortfolioTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TransactionsWithSameSharesDifferentPrices()
        {
            // arrange
            long expectedShares = 200;
            double expecctedPrice = 15;

            IPortfolioDao portfolioDao = new PortfolioDao();
            portfolioDao.Save(new PortfolioDataEntity("GPRO", 100, 10));
            portfolioDao.Save(new PortfolioDataEntity("GPRO", 100, 20));

            // act
            IPortfolioDataEntity portfolio = portfolioDao.GetBySymbol("GPRO");

            // assert
            long actualShares = portfolio.Shares;
            double actualPrice = portfolio.Price;

            Assert.AreEqual(expectedShares, actualShares, 0.0, "Cummulative Shares computed incorrectly");
            Assert.AreEqual(expecctedPrice, actualPrice, 0.0, "Weighted Average Price computed incorrectly");
        }

        [TestMethod]
        public void TransactionsWithDifferentSharesSamePrices()
        {
            // arrange
            long expectedShares = 150;
            double expecctedPrice = 20;

            IPortfolioDao portfolioDao = new PortfolioDao();
            portfolioDao.Save(new PortfolioDataEntity("GPRO", 50, 20));
            portfolioDao.Save(new PortfolioDataEntity("GPRO", 100, 20));

            // act
            IPortfolioDataEntity portfolio = portfolioDao.GetBySymbol("GPRO");

            // assert
            long actualShares = portfolio.Shares;
            double actualPrice = portfolio.Price;

            Assert.AreEqual(expectedShares, actualShares, 0.0, "Cummulative Shares computed incorrectly");
            Assert.AreEqual(expecctedPrice, actualPrice, 0.0, "Weighted Average Price computed incorrectly");
        }

        [TestMethod]
        public void TransactionsWithDifferentSharesDifferentPrices()
        {
            // arrange
            long expectedShares = 300;
            double expecctedPrice = 20;

            IPortfolioDao portfolioDao = new PortfolioDao();
            portfolioDao.Save(new PortfolioDataEntity("GPRO", 200, 10));
            portfolioDao.Save(new PortfolioDataEntity("GPRO", 100, 40));

            // act
            IPortfolioDataEntity portfolio = portfolioDao.GetBySymbol("GPRO");

            // assert
            long actualShares = portfolio.Shares;
            double actualPrice = portfolio.Price;

            Assert.AreEqual(expectedShares, actualShares, 0.0, "Cummulative Shares computed incorrectly");
            Assert.AreEqual(expecctedPrice, actualPrice, 0.0, "Weighted Average Price computed incorrectly");
        }
    }
}
