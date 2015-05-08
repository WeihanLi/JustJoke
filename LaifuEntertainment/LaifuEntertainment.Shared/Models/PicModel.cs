using System;
using System.Collections.Generic;
using System.Text;

namespace LaifuEntertainment.Models
{
    public class PicModel
    {
        /// <summary>
        /// 图片标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 源url
        /// </summary>
        public string sourceurl { get; set; }

        /// <summary>
        /// 图片网址
        /// </summary>
        public string thumburl { get; set; }
        
        /// <summary>
        /// 图片高度
        /// </summary>
        public string height { get; set; }
        
        /// <summary>
        /// 图片宽度
        /// </summary>
        public string width { get; set; }
    }
}
