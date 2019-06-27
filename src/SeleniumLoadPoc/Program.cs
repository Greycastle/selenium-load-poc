using System;
using System.Diagnostics;
using CodenameGenerator;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumLoadPoc
{
    public class Program
    {
        public static int Main()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                RunTest();
                Console.WriteLine($"Completed in {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception e)
            {
                Console.WriteLine("Test run failed!");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return 1;
            }

            return 0;
        }

        private static ChromeOptions GetDriverOptions()
        {
            var options = new ChromeOptions();
            options.AddArgument("headless");
            options.AddArgument("window-size=800,600");
            options.AddArgument("disable-gpu");
            options.AddArgument("no-sandbox");
            options.AddArgument("ignore-certificate-errors");
            options.AcceptInsecureCertificates = true;
            return options;
        }

        private static void RunTest()
        {
            var generator = new Generator();
            var hostname = Environment.GetEnvironmentVariable("TARGET_HOSTNAME") ?? "localhost";
            var codename = generator.Generate();

            using (IWebDriver driver = new ChromeDriver(GetDriverOptions()))
            {
                var testSteps = new TestSteps(driver, $"https://{hostname}:5001");
                testSteps.OpenWebSite();

                testSteps.SetInput(codename);
                testSteps.Submit();
                testSteps.CheckThatOutputIs(codename);

                driver.Close();
            }
        }
    }

    public class TestSteps
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _rootUrl;

        public TestSteps(IWebDriver driver, string rootUrl)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            _rootUrl = rootUrl;
        }

        private IWebElement FindElementById(string id)
        {
            return _wait.Until(webdriver => webdriver.FindElement(By.Id(id)));
        }

        public void OpenWebSite()
        {
            _driver.Navigate().GoToUrl(_rootUrl);
            ShouldBeEqual("Home Page - SeleniumLoadPoc.TestApp", _driver.Title, "title");
        }

        private static void ShouldBeEqual(string expected, string actual, string propertyName)
        {
            if (actual != expected)
            {
                throw new Exception($"Expected {propertyName} '{actual}' to be '{expected}'");
            }
        }

        public void SetInput(string value)
        {
            FindElementById("input").SendKeys(value);
        }

        public void Submit()
        {
            FindElementById("submit").Click();
        }

        public void CheckThatOutputIs(string value)
        {
            var output = FindElementById("output");
            ShouldBeEqual(value, output.Text, "output text");
        }
    }
}
