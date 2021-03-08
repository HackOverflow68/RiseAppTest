using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace RiseAppTestProject
{
    [TestFixture]
    public class AutomationTests
    {
        #region variables
        ChromeDriver driver;
        IWebElement txtUserName, txtPassword, txtNationalityName, txtDescription, 
                    btnLogin, drpUserProfiledropdown, btnLogout, navMenu, Lookups, 
                    NationalityLookup, NationalityLink, NationalityDescription,
                    btnAdd, btnSave, btnEdit, btnDelete, btnDeleteLink, alertElement;
        #endregion

        #region set up and tear down methods
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://staging.riseapp.co.za/");
        }

        [TearDown]
        public void Close()
        {
            driver.Close();
        }
        #endregion

        #region Tests
        [TestCase("pinkshrub@riseapp.co.za", "adminadmin123")]
        [TestCase("yellowgrass@riseapp.co.za", "adminadmin123")]
        [TestCase("greentree@riseapp.co.za", "adminadmin123")]
        public void LoginTestHappyPath(string Username, string Password)
        {          
            login(Username, Password);

            //Web Elements necessary to logout
            drpUserProfiledropdown = driver.FindElementByCssSelector(".dropdown-user-link");
            btnLogout = driver.FindElementByXPath("//a[contains(.,'Logout')]");

            //If the user profile drop down is visible then the login operation passes.
            if (drpUserProfiledropdown.Displayed == true)
            {
                //logout activity
                drpUserProfiledropdown.Click();
                btnLogout.Click();
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void Nationality_CRUD_Test_Scenario()
        {
            //1. Login
                login("pinkshrub@riseapp.co.za", "adminadmin123");
                ImplicitWait(10);

            //2. Navigate to Nationality
                navMenu = driver.FindElementByCssSelector(".d-none .ft-menu");
                navMenu.Click();

                //Lookups = driver.FindElementByXPath("//span[contains(.,'Lookups')]");
                Lookups = driver.FindElementByCssSelector(".has-sub .menu-title");
                Lookups.Click();

                NationalityLookup = driver.FindElementByLinkText("Nationalities");
                NationalityLookup.Click();

                //Implicit wait for Nationality list to load
                ImplicitWait(10);

            //3. Create New Nationality
                btnAdd = driver.FindElementByXPath("//span[contains(.,'Add Nationality')]");
                btnAdd.Click();

                //Implicit wait to load create page
                ImplicitWait(5);

                txtNationalityName = driver.FindElementById("id_name");
                txtDescription = driver.FindElementById("id_description");

                txtNationalityName.SendKeys("Villager");
                txtDescription.SendKeys("Villager from MineCraft");
                btnSave = driver.FindElementByName("save");
                btnSave.Click();

                //Implicit wait for Nationality list to load
                ImplicitWait(5);

            //4. View Created Nationality
                
                //Get the current count in the list (This will help getting the specific selector)
                string count = driver.FindElementByCssSelector(".media-body > h3").Text;

                //Check that the new Nationality has beeen created and is being displayed in the list
                NationalityLink = driver.FindElementByLinkText("Villager");
                Assert.IsTrue(NationalityLink.Displayed == true);

            //5. Edit Nationality
                NationalityLink.Click();

                //Implicit wait to load the edit page
                ImplicitWait(5);
                btnEdit = driver.FindElementByXPath("//span[contains(.,'Edit')]");
                btnEdit.Click();

                //Update Nationality
                txtNationalityName = driver.FindElementById("id_name");
                txtDescription = driver.FindElementById("id_description");
                txtNationalityName.SendKeys(Keys.Control + "a" + Keys.Control);
                txtNationalityName.SendKeys("Zillager");

                txtDescription.SendKeys(Keys.Control + "a" + Keys.Control);
                txtDescription.SendKeys("Zillager from MineCraft");
                btnSave = driver.FindElementByName("save");
                btnSave.Click();

                //Implicit wait for Nationality list to load
                ImplicitWait(5);

                //Check that the new Nationality has beeen created and is being displayed in the list
                NationalityLink = driver.FindElementByLinkText("Zillager");
                NationalityDescription = driver.FindElementByXPath("//td[contains(.,'Zillager from MineCraft')]");
                Assert.IsTrue(NationalityLink.Displayed == true);
                Assert.IsTrue(NationalityDescription.Displayed == true);

            //6. Delete Nationality
                btnDeleteLink = driver.FindElementByCssSelector($".even:nth-child({count}) .fa-trash");
                btnDeleteLink.Click();

                btnDelete = driver.FindElementByXPath("//button[contains(.,' Confirm')]");
                btnDelete.Click();

                //Implicit wait for Nationality list to load
                ImplicitWait(5);

                alertElement = driver.FindElementByCssSelector(".alert");
                Assert.IsTrue(alertElement.Text.Contains("Nationality successfully Deleted"));
        }
        #endregion

        #region Helper functions
        public void login(string Username, string Password)
        {
            txtUserName = driver.FindElementById("id_username");
            txtPassword = driver.FindElementById("id-password");
            btnLogin = driver.FindElementByXPath("//button[contains(.,'Login')]");

            //Login Activity
            txtUserName.SendKeys(Username);
            txtPassword.SendKeys(Password);
            btnLogin.Click();

            //Implicit wait to compute login
            ImplicitWait(3);
        }

        public void ImplicitWait(int seconds)
        {
            //Implicit wait to compute login
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }
        #endregion
    }
}