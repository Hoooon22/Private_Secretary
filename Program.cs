using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
// CognitiveServices Library
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
// Selenium Library
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Private_Secretary
{
    class Program
    {
        // Speech to Text - from Mic
        async static Task<string> FromMic(SpeechConfig speechConfig)
        {
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

            Console.WriteLine("Speak into your microphone.");
            var result = await recognizer.RecognizeOnceAsync();
            // Console.WriteLine($"RECOGNIZED: Text={result.Text}");
            // error
            switch (result.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    // Console.WriteLine($"RECOGNIZED: Text={result.Text}");
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the speech key and location/region info?");
                    }
                    break;
            }
            return result.Text;
        }

        // Interpretation using speech str
        async static Task Interpretation(string str)
        {
            if (str.Contains("Search") || str.Contains("search")) // Internet Search 
            {
                try
                {
                    using (IWebDriver cDriver = new ChromeDriver())
                    {
                        str = Regex.Replace(str, "[.]", string.Empty);
                        string[] search_word = str.Split(' ');

                        // connect URL
                        cDriver.Url = "https://www.google.com";

                        // search
                        var searchBox = cDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/form/div[1]/div[1]/div[1]/div[2]/div[2]/input"));
                        searchBox.SendKeys(search_word[1]);
                        searchBox.SendKeys(Keys.Enter);
                        cDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
            }
        }

        async static Task Main(string[] args)
        {
            var speech_str = "";
            // 음성 SDK를 사용하여 음성 서비스를 호출
            var speechConfig = SpeechConfig.FromSubscription("e1d9172eea8241a7aa993ed74df9b886", "koreacentral");

            speech_str = await FromMic(speechConfig);
            Console.WriteLine($"speech_str Test : {speech_str}");
            await Interpretation(speech_str);
        }
    }
}