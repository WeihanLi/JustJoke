using System;
using System.Linq;
using Windows.Media.SpeechSynthesis;
using System.Threading.Tasks;

namespace LaifuEntertainment.Helper
{
    public class SpeechHelper
    {
        private static string voiceId = Windows.ApplicationModel.Store.CurrentApp.AppId.ToString();

        public static async void TextToSpeech(string text,VoiceInformation voiceInfo)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            //IEnumerable<VoiceInformation> voices = SpeechSynthesizer.AllVoices;
            //IEnumerable<VoiceInformation> voices = from voice in SpeechSynthesizer.AllVoices
            //                                       where voice.DisplayName == "zh-CN" //&& voice.Gender == VoiceGender.Female
            //                                       select voice;
            //foreach (VoiceInformation voice in voices)
            //{
            //    if (voice.DisplayName == "zh-CN")
            //    {
            //        synthesizer.Voice = voice;
            //        break;
            //    }
            //}

            //if (voiceInfo!=null)
            //{
                
            //}
            //else
            //{
            //    await new Windows.UI.Popups.MessageDialog("There is no suitable speech find").ShowAsync();
            //    return;
            //}
            synthesizer.Voice = voiceInfo;
            SpeechSynthesisStream synthesizerStream = await synthesizer.SynthesizeTextToStreamAsync(text);
            Windows.UI.Xaml.Controls.MediaElement media = new Windows.UI.Xaml.Controls.MediaElement();
            media.SetSource(synthesizerStream, synthesizerStream.ContentType);
            media.Play();
        }

        //public static async Task<string> TextToSpeech(string text)
        //{
        //    TTSService tts = new TTSService();
        //    return tts.GetTTS(text, voiceId);
        //}
    }
}
