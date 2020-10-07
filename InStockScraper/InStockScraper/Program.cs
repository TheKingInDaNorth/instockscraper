using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mail;

namespace InStockScraper
{
    class Program
    {
        static string nvdia = "https://www.nvidia.com/en-us/shop/geforce/gpu/?page=2&limit=9&locale-en-us=&category=GPU&gpu=RTX%203080&locale=en-us";
        static string bestbuy = "https://www.bestbuy.com/site/searchpage.jsp?_dyncharset=UTF-8&acampID=0%2C0&id=pcat17071&iht=y&irclickid=zMI0JrSrqxyOT9N0WIXSvXJOUkiXtN3RGSuWRY0%2C1Fqys%3ASqWxyOUPr0RHQK3XRkUkiXLO3NLwagQo0&irgwc=1%2C1&keys=keys&ks=960&loc=Future%20PLC.%2CFuture%20PLC.&mpid=221109%2C221109&ref=198%2C198%2C198&sc=Global&st=rtx%203080&type=page&usc=All%20Categories";
        static string anthonyEmail = "muuningaming@gmail.com";
        static string testEmail = "itztuanvo@yahoo.com";
        static void Main(string[] args)
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArguments("--headless");
           
            FirefoxDriver driver = new FirefoxDriver(Environment.CurrentDirectory, options);

            //Console.WriteLine("Program running. Enter: Ctrl + C to exit program");
            //while(true)
            //{
            //    if (checkPage(driver, "https://www.nvidia.com/en-us/shop/geforce/?page=1&limit=9&locale=en-us&search=3070", "/html/body/app-root/product/div[1]/div[1]/div[2]/div/product-details/div/div/div/div[3]/div[2]/div[2]/div[2]/a", "Notify Me") 
            //        || checkPage(driver, "https://www.bestbuy.com/site/searchpage.jsp?st=rtx+3070&_dyncharset=UTF-8&_dynSessConf=&id=pcat17071&type=page&sc=Global&cp=1&nrp=&sp=&qp=&list=n&af=true&iht=y&usc=All+Categories&ks=960&keys=keys", "/html/body/div[4]/main/div[10]/div/div/div/div/div/div/div[2]/div[2]/div[5]/div/div/div/div[6]/ol/li[2]/div/div/div/div/div/div[2]/div[2]/div[3]/div/div/div/div/div/button", "Coming Soon"))
            //        break;
            //}
            //uncomment in final product
            //SendEmail("muuningaming@gmail.com", "BACK IN STOCK", "BACK IN STOCK");

            string[] websites = { "https://www.nvidia.com/en-us/shop/geforce/?page=1&limit=9&locale=en-us&search=3070", "https://www.bestbuy.com/site/searchpage.jsp?st=rtx+3070&_dyncharset=UTF-8&_dynSessConf=&id=pcat17071&type=page&sc=Global&cp=1&nrp=&sp=&qp=&list=n&af=true&iht=y&usc=All+Categories&ks=960&keys=keys" };
            string[] xpaths = { "/html/body/app-root/product/div[1]/div[1]/div[2]/div/product-details/div/div/div/div[3]/div[2]/div[2]/div[2]/a", "/html/body/div[4]/main/div[10]/div/div/div/div/div/div/div[2]/div[2]/div[5]/div/div/div/div[6]/ol/li[2]/div/div/div/div/div/div[2]/div[2]/div[3]/div/div/div/div/div/button" };
            string[] textMatchs = { "Notify Me" , "Coming Soon" };
            string[] classes = { "featured-buy-link", "btn-disabled" };

            checkPageClasses(driver, websites, classes, textMatchs, 2);
            //checkPageXPaths(driver, websites, xpaths, textMatchs, 2);
            //checkPageTest(driver, "https://www.nvidia.com/en-us/shop/geforce/?page=1&limit=9&locale=en-us&search=3070", "notify-me-btn", "notify me");
        }

        static void SendEmail(string toEmail, string message, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient();

                mail.From = new MailAddress("scrapernoscraper@gmail.com");
                mail.To.Add(new MailAddress(toEmail));
                mail.Subject = message;
                mail.Body = body;

                smtpServer.Host = "smtp.gmail.com";
                smtpServer.Port = 587;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new NetworkCredential("scrapernoscraper@gmail.com", "sukix916");
                smtpServer.EnableSsl = true;
                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpServer.Send(mail);
                Console.WriteLine("Email sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Email failed to send");
            }
        }

        static bool checkPageTest(FirefoxDriver driver, string url, string xpath, string textMatch)
        {
            driver.Url = url;
            //Console.WriteLine("Hi");
            try
            {
                ReadOnlyCollection <IWebElement> buylinks = driver.FindElementsByClassName(xpath);

                foreach (var buylink in buylinks)
                {
                    if (buylink.Text.ToLower().Equals(textMatch.ToLower()))
                    {
                        Console.WriteLine(buylink.Text);
                        //SendEmail(testEmail, "Nvdia has stock", url);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //SendEmail(testEmail, "Error need to restart program");
                return true;
            }
            return false;
        }

        static void checkPageClasses(FirefoxDriver driver, string [] websites, string [] classes, string [] textMatchs, int size)
        {
            bool emailSent = false;
            Console.WriteLine("The Program is running.....");
            Console.WriteLine("Type Ctrl+c to exit");
            while(true)
            {
                for(var i = 0; i < size; i++)
                {
                    driver.Url = websites[i];
                    //Console.WriteLine("Hi");
                    try
                    {
                        ReadOnlyCollection<IWebElement> buylinks = driver.FindElementsByClassName(classes[i]);
                        foreach (var buylink in buylinks)
                        {
                            if (!buylink.Text.ToLower().Equals(textMatchs[i].ToLower()))
                            {
                                //Console.WriteLine(buylink.Text);
                                SendEmail(testEmail, "card in stock", websites[i]);
                                //emailSent = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SendEmail(testEmail, "Error need to restart program", "check website to be sure" + websites[i]);
                        //Console.WriteLine(ex.Message);
                        emailSent = true;
                    }
                }
                if(emailSent)
                {
                    Console.WriteLine("email sent, program stopping");
                    break;
                }
            }
        }

        static void checkPageXPaths(FirefoxDriver driver, string[] websites, string[] xpaths, string[] textMatchs, int size)
        {
            bool emailSent = false;
            while (true)
            {
                for (var i = 0; i < size; i++)
                {
                    driver.Url = websites[i];
                    Console.WriteLine("Hi");
                    try
                    {
                        ReadOnlyCollection<IWebElement> buylinks = driver.FindElementsByXPath(xpaths[i]);
                        foreach (var buylink in buylinks)
                        {
                            if (buylink.Text.ToLower().Equals(textMatchs[i].ToLower()))
                            {
                                Console.WriteLine(buylink.Text);
                                //SendEmail(testEmail, $"Card in STOCK: go to {websites[i]}");
                                //emailSent = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //SendEmail(testEmail, "Error need to restart program");
                        Console.WriteLine(ex.Message);
                        emailSent = true;
                    }
                }
                if (emailSent)
                {
                    Console.WriteLine("email sent, program stopping");
                    break;
                }
            }
        }
    }
}
//static void checkPage(FirefoxDriver driver, string[] websites, string[] xpaths, string[] textMatchs, int size)
//{
//    bool emailSent = false;
//    while (true)
//    {
//        for (var i = 0; i < size; i++)
//        {
//            driver.Url = websites[i];
//            Console.WriteLine("Hi");
//            try
//            {
//                ReadOnlyCollection<IWebElement> buylinks = driver.FindElementsByXPath(xpaths[i]);
//                foreach (var buylink in buylinks)
//                {
//                    if (buylink.Text.ToLower().Equals(textMatchs[i].ToLower()))
//                    {
//                        Console.WriteLine(buylink.Text);
//                        //SendEmail(testEmail, $"Card in STOCK: go to {websites[i]}");
//                        //emailSent = true;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                //SendEmail(testEmail, "Error need to restart program");
//                Console.WriteLine(ex.Message);
//                emailSent = true;
//            }
//        }
//        if (emailSent)
//        {
//            Console.WriteLine("email sent, program stopping");
//            break;
//        }
//    }
//}