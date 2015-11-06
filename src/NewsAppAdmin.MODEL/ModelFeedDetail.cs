using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.MODEL
{
    public class ModelFeedDetail
    {
        public string FeedDetailId { get; set; }
        public string PostTitle { get; set; }
        public string ShortDescription { get; set; }
        public string PublishDate { get; set; }
        public string RssImage { get; set; }
        public string DetailPageURL { get; set; }
        public string DetailPageImage { get; set; }
        public string DetailPagePostDetail { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
