using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace Private_Secretary
{
    class Program
    {
        async static Task FromMic(SpeechConfig speechConfig)
        {

        }

        async static Task Main(string[] args)
        {
            // 음성 SDK를 사용하여 음성 서비스를 호출
            var speechConfig = SpeechConfig.FromSubscription("<paste-your-speech-key-here>", "<paste-your-speech-location/region-here>");
        }
    }
}
