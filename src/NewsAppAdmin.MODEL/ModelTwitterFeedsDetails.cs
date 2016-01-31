using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.MODEL
{
    public class ModelTwitterFeedsDetails
    {
        public ModelTwitterFeedsDetails()
        { 
        }
        
        public long TwitterPageId { get; set; }
        public string FeedText{ get; set; }
        public string FeedLanguage { get; set; }
        public string FeedTextDetail { get; set; }
        public string FeedDetailPageURL { get; set; }
        public long FeedPostedtId { get; set; }
        public DateTime FeedPostDate { get; set; }
        public List<ModelFeedMultimedia> FeedMultimediaList { get; set; }
    }
}
