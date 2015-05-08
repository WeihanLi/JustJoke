using System.Collections.Generic;
using System.Text;

namespace LaifuEntertainment.Models
{
    public class JokeModel
    {
        private static Dictionary<string, string> dic = new Dictionary<string, string>() { { "<br/>","\r\n"}, { "&nbsp;"," "} };
        private string _content;
        public string title { get; set; }

        public string content { get { return _content;} set { _content = HandleContent(value); } }

        public string url { get; set; }

        public string poster { get; set; }

        private string HandleContent(string val)
        {
            string tmp = val;
            foreach (string item in dic.Keys)
            {
                if (tmp.Contains(item))
                {
                    tmp =val.Replace(item, dic[item]);
                }
            }
            return tmp;
        }
    }
}
