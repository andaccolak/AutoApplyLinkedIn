using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AutoApplyLinkedIn
{
    class LinkedInJobApplyBot
    {
        private IWebDriver driver;
        private List<string> keywords;
        private int totalJobs = 0;
        private int appliedJobs = 0;
        private int skippedJobs = 0;

        public LinkedInJobApplyBot(List<string> keywords)
        {
            this.keywords = keywords;

            var options = new ChromeOptions();
            options.AddArgument("--user-data-dir=C:/Users/colak/AppData/Local/Google/Chrome/User Data");
            options.AddArgument("--profile-directory=Default");
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--remote-debugging-port=9222");

            driver = new ChromeDriver(options);
        }

        public void Start()
        {
            var Url = ""; // LinkedIn  search URL
            driver.Navigate().GoToUrl(Url);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(drv => drv.FindElements(By.CssSelector("ul.YtNdVOCZsWKSiVMvHkvCZxigQKCdECLPYTFM > li")).Count > 0);

            var jobCards = driver.FindElements(By.CssSelector("ul.YtNdVOCZsWKSiVMvHkvCZxigQKCdECLPYTFM > li")).ToList();
            totalJobs = jobCards.Count;

            Console.WriteLine($"Toplam iş ilanı bulundu: {totalJobs}");

            for (int i = 0; i < totalJobs; i++)
            {
                try
                {
                    new Actions(driver).MoveToElement(jobCards[i]).Click().Perform();
                    Thread.Sleep(3000);

                    string descriptionText = GetDescriptionText();
                    Console.WriteLine($"\n--- İlan Açıklaması [{i + 1}] ---\n{descriptionText}\n");

                    bool hasKeyword = ContainsAnyKeyword(descriptionText);

                    if (hasKeyword)
                    {
                        Console.WriteLine($"[{i + 1}] Anahtar kelime bulundu.");

                        if (ClickIfExists(By.XPath("//button[.//span[text()='Kolay Başvuru']]")))
                        {
                            Console.WriteLine("Kolay Başvuru butonuna tıklandı.");
                            Thread.Sleep(2000);

                            while (true)
                            {
                                if (ClickIfExists(By.XPath("//button[.//span[text()='İleri']]")))
                                {
                                    Console.WriteLine("İleri butonuna basıldı.");
                                    Thread.Sleep(1500);
                                    continue;
                                }
                                else if (ClickIfExists(By.XPath("//button[.//span[text()='İncele']]")))
                                {
                                    Console.WriteLine("İncele butonuna basıldı.");
                                    Thread.Sleep(1500);
                                    continue;
                                }
                                else if (ClickIfExists(By.XPath("//button[.//span[text()='Başvuruyu gönder']]")))
                                {
                                    Console.WriteLine("Başvuruyu gönder butonuna basıldı.");
                                    appliedJobs++;
                                    Thread.Sleep(2000);

                                    // Bitti butonuna bas
                                    if (ClickIfExists(By.XPath("//button[.//span[text()='Bitti']]")))
                                    {
                                        Console.WriteLine("Başvuru tamamlandı, Bitti butonuna basıldı.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Bitti butonu bulunamadı.");
                                    }

                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("İleri/İncele/Gönder butonları bulunamadı, işlem iptal.");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Kolay Başvuru butonu bulunamadı, ilan atlandı.");
                            skippedJobs++;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[{i + 1}] Anahtar kelime yok, ilan atlandı.");
                        skippedJobs++;
                    }

                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{i + 1}] Hata: {ex.Message}");
                    skippedJobs++;
                }
            }

            PrintSummary();
            driver.Quit();
        }

        private string GetDescriptionText()
        {
            try
            {
                var containers = driver.FindElements(By.CssSelector(
                    "div.jobs-description-content__text, div.jobs-description__container, div.show-more-less-html__markup, div.mt4"));
                var fullText = string.Join(" ", containers.Select(c => c.Text));
                return fullText.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool ContainsAnyKeyword(string text)
        {
            return keywords.Any(k => text.Contains(k, StringComparison.OrdinalIgnoreCase));
        }

        private bool ClickIfExists(By by)
        {
            try
            {
                var element = driver.FindElement(by);
                if (element.Displayed && element.Enabled)
                {
                    element.Click();
                    return true;
                }
            }
            catch { }
            return false;
        }

        private void PrintSummary()
        {
            Console.WriteLine("\n--- İŞLEM TAMAMLANDI ---");
            Console.WriteLine($"Toplam ilan: {totalJobs}");
            Console.WriteLine($"Başvurulan ilan: {appliedJobs}");
            Console.WriteLine($"Atlanan ilan: {skippedJobs}");
        }
    }
}
