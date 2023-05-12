using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace InfoTrackSEO.UiTests.UIIntegrationTests
{
    // spin up the api project and web project before running tests
    public class UIIntegrationTests : IDisposable
    {
        private readonly IWebDriver _driver;

        public UIIntegrationTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://localhost:54321");
        }

        [Fact]
        public void PageTitle_IsCorrect()
        {
            Assert.Equal("InfoTrack SEO Checker", _driver.Title);
        }

        [Fact]
        public void SubmitSearch_FormIsSubmitted()
        {
            var keywordsInput = _driver.FindElement(By.Id("keywords"));
            var urlInput = _driver.FindElement(By.Id("url"));
            var searchEngineSelect = _driver.FindElement(By.Id("searchEngine"));
            var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            keywordsInput.SendKeys("efiling integration");
            urlInput.SendKeys("www.infotrack.com");
            searchEngineSelect.SendKeys("Google");
            submitButton.Click();
        }

        [Fact]
        public void SearchEngineDropdown_ContainsGoogle()
        {
            var searchEngineSelect = _driver.FindElement(By.Id("searchEngine"));
            var options = searchEngineSelect.FindElements(By.TagName("option"));

            Assert.Equal(1, options.Count);
            Assert.Contains(options, option => option.Text == "Google");
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}
