using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeetRaids.Controllers;
using DataAccessLayer;
using System.Configuration;
using Moq;

namespace LeetRaids.Tests.Controllers
{
    /// <summary>
    /// Summary description for AccountTest
    /// </summary>
    [TestClass]
    public class AccountTest
    {
        public AccountTest()
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
        public void Test_Register()
        {
            //string connStr = ConfigurationManager.ConnectionStrings["DataAccessLayer.Properties.Settings.LeetRaidsConnectionString"].ToString();
            //AccountInterface testAccountInterface = new AccountInterface(connStr);
            //GameInterface testGameInterFace = new GameInterface(connStr);
            //AccountController testAccountController = new AccountController(testAccountInterface, testGameInterFace);

            //Member testMem = new Member() { Password = "123456", PlayTimeEnd = new TimeSpan(12, 0, 0), PlayTimeStart = new TimeSpan(17, 0, 0), TimeZone = "-8.0", CreateDT = DateTime.Now };
            //RegistrationTestValues regValues = new RegistrationTestValues(@"C:\Users\S\Documents\Visual Studio 2008\Projects\LeetRaids\LeetRaids.Tests\TestValues.xml");
            //testMem.Email = regValues.Email;

            //testAccountController.Register(testMem);
            //Member loggedInMem = testAccountInterface.Login(new Member() { Email = testMem.Email, Password = testMem.Password });

            //Assert.IsNotNull(loggedInMem, "Login Failed on Test Reg Member");
        }
    }
}
