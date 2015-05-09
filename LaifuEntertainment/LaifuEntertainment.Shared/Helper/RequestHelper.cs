using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LaifuEntertainment.Helper
{
    public class RequestHelper
    {
        private const string jokeUrl= "http://api.laifudao.com/open/xiaohua.json";
        private const string picUrl= "http://api.laifudao.com/open/tupian.json";

        /// <summary>
        ///  加载笑话
        /// </summary>
        /// <returns>返回数据</returns>
        public async Task<List<Models.JokeModel>> LoadJokes()
        {
            //
            string response = await DoGetRequestAsync(jokeUrl);
            if (response== "Fail")
            {
                return null;
            }
            //设置本地存储
            //await SetLocalData(response);
            //将json字符串转换为对象
            List<Models.JokeModel> jokes = Helper.ConverterHelper.JsonToObj<List<Models.JokeModel>>(response);
            return jokes;
        }

        public async void RequestJokes()
        {
            string response = await DoGetRequestAsync(jokeUrl);
            //设置本地存储
            await SetLocalData(response);
        }

        public async Task SetLocalData(string content)
        {
            StorageFile file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("jokes.json");
            if (file != null)
            {
                    if (!String.IsNullOrEmpty(content))
                    {
                        await FileIO.WriteTextAsync(file, content);
                    }
            }
        }

        /// <summary>
        /// 加载本地数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<Models.JokeModel>> LoadLocalJokesData()
        {
            StorageFile file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("jokes.json");
            if (file != null)
            {
                string fileContent = await FileIO.ReadTextAsync(file);
                //将json字符串转换为对象
                List<Models.JokeModel> jokes = Helper.ConverterHelper.JsonToObj<List<Models.JokeModel>>(fileContent);
                return jokes;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Models.PicModel>> LoadPics()
        {
            string response = await DoGetRequestAsync(picUrl);
            List<Models.PicModel> pics = Helper.ConverterHelper.JsonToObj<List<Models.PicModel>>(response);
            return pics;
        }

        /// <summary>
        /// 异步发送GET请求
        /// </summary>
        /// <param name="url">要请求的URL</param>
        /// <returns>请求所返回的字符串</returns>
        public static async Task<string> DoGetRequestAsync(string url)
        {
            string responseText = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                // Set the Method property of the request to GET.
                request.Method = "GET";
                //
                request.Headers["UserAgent"] = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.99 Safari/537.36";
                //
                WebResponse response = await request.GetResponseAsync();
                using (System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    responseText = myreader.ReadToEnd();
                }
            }
            catch (WebException)
            {
                responseText = "Fail";
            }
            catch (Exception)
            {
                responseText = "Fail";
            }
            return responseText;
        }

        public static async Task<string> DoPostRequestAsync(string url, string postStr)
        {
            string responseText = null;
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                // Set the Method property of the request to POST.
                request.Method = "POST";
                //
                byte[] byteArray = Encoding.UTF8.GetBytes(postStr);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                //request.ContentLength = byteArray.Length;
                // Get the request stream.
                using (Stream dataStream = await request.GetRequestStreamAsync())
                {
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                request.Headers["UserAgent"] = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.99 Safari/537.36";
                //request.
                WebResponse response = await request.GetResponseAsync();
                using (StreamReader myreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = myreader.ReadToEnd();
                }
            }
            catch (WebException)
            {
                responseText = "Fail";
            }
            catch (Exception)
            {
                responseText = "Fail";
            }
            return responseText;
        }
    }
}
