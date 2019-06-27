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
        public static void Main()
        {
            var stopwatch = Stopwatch.StartNew();
            var generator = new Generator();

            var hostname = Environment.GetEnvironmentVariable("TARGET_HOSTNAME") ?? "localhost";

            var codename = generator.Generate();
            var options = new ChromeOptions();
            options.AddArgument("headless");
            options.AddArgument("window-size=800,600");
            options.AddArgument("disable-gpu");
            options.AddArgument("no-sandbox");
            options.AddArgument("ignore-certificate-errors");

            options.AcceptInsecureCertificates = true;
            var driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            driver.Navigate().GoToUrl($"https://{hostname}:5001");
            const string expectedTitle = "Home Page - SeleniumLoadPoc.TestApp";
            if (driver.Title != expectedTitle)
            {
                throw new Exception($"Expected title '{driver.Title}' to be '{expectedTitle}'");
            }
            wait.Until(webdriver => webdriver.FindElement(By.Id("input"))).SendKeys(codename);
            driver.FindElement(By.Id("submit")).Click();
            var outputElement = wait.Until(webDriver => webDriver.FindElement(By.Id("output")));
            if (codename != outputElement.Text)
            {
                throw new Exception($"Expected output text '{outputElement.Text}' to be '{codename}'");
            }

            driver.Close();

            Console.WriteLine($"Completed in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
