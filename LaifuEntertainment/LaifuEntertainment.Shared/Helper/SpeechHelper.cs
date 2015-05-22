using System;
using System.Linq;
using Windows.Media.SpeechSynthesis;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.IO;
using Windows.UI.Xaml.Controls;
using System.Net;
using System.Collections.Generic;

namespace LaifuEntertainment.Helper
{
    public class SpeechHelper
    {
        private const string speechUrlFormat = "http://api.microsofttranslator.com/v2/http.svc/speak?appId=TG3vML1m7eGuBDvMfmaDzvEnpjRLUZj67PUbRMf1wIH4*&language=zh-CHS&format=audio/mp3&options=MinSize&text={0}";

        public static async Task<SpeechSynthesisStream> GetTTSStream(string text,VoiceInformation voiceInfo)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Voice = voiceInfo;
            SpeechSynthesisStream synthesizerStream = await synthesizer.SynthesizeTextToStreamAsync(text);
            return synthesizerStream;
        }

        private static async Task<Stream> RequestForSpeechStream(string text)
        {
            //string url = String.Format(speechUrlFormat, text);
            HttpWebRequest request = WebRequest.CreateHttp(String.Format(speechUrlFormat,text));
            // 设置请求方式 .
            request.Method = "GET";
            //设置ContentType
            request.ContentType = "application/x-www-form-urlencoded";
            //设置Header，防止服务器验证返回错误
            request.Headers["UserAgent"] = "Mozilla/5.0 (Windows NT 10.0; Trident/7.0; rv:11.0) like Gecko";
            //设置Referer
            request.Headers["Referer"] = "http://www.bing.com/translator/";
            request.Headers["Host"] = "api.microsofttranslator.com";
            //Do request
            WebResponse response = await request.GetResponseAsync();
            return response.GetResponseStream();
        }

        //Todo:长文本分段处理
        public static async Task<InMemoryRandomAccessStream> GetSpeechStream(string text)
        {
            Stream stream = null;
            string tmp = text.Substring(0,500);
            return await StreamToIRandomAccessStreamAsync(stream);
        }

        public static async void SpeakTextAsync(string text)
        {
            MediaElement media = new MediaElement();
            media.AutoPlay = true;
            media.Volume = 100;
            Stream stream = await RequestForSpeechStream(text);
            media.SetSource(await StreamToIRandomAccessStreamAsync(stream), "audio/mpeg");
            media.Play();
        }

        /// <summary>
        /// 流类型的 转换
        /// </summary>
        /// <param name="stream">Stream 对象</param>
        /// <returns>IRandomAccessStream对象</returns>
        private static async Task<InMemoryRandomAccessStream> StreamToIRandomAccessStreamAsync(Stream stream)
        {
            InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();
            Stream s = ras.AsStreamForWrite();
            await stream.CopyToAsync(s);
            //需要提交数据
            await s.FlushAsync();
            return ras;
        }

        internal static async Task<SpeechSynthesisStream> GetTTSStream(string text)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            if (synthesizer.Voice.Language!="zh-CN")
            {
                IEnumerable<VoiceInformation> voices = from voice in SpeechSynthesizer.AllVoices
                                                                                   where voice.Language=="zh-CN"
                                                                                   select voice;
                if (voices.Count() > 0)
                {
                    synthesizer.Voice = voices.ElementAt(0);
                }
                else
                {
                    return null;
                }
            }
            SpeechSynthesisStream synthesizerStream = await synthesizer.SynthesizeTextToStreamAsync(text);
            return synthesizerStream;
        }
    }
}
