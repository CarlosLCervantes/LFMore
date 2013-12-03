using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using System.Xml.Linq;


namespace LeetRaids.Tests.DataAccessLayer
{
    /// <summary>
    /// Summary description for DAL
    /// </summary>
    [TestClass]
    public class DAL
    {
        public DAL()
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
        public void GetError()
        {
            List<string> errors = new List<string> { "NotAUniqueEmailAddress" };
            try
            {
                throw new BusinessLogicException(errors);
            }
            catch (BusinessLogicException ex)
            {
                //Assert.IsTrue(ex.logicErrors.Contains("The Email Address you requested is already taken by a current user."), 
                //    "Checking that errorNames are succesfully being translated into descriptions") ;
            }
        }
    }
}
