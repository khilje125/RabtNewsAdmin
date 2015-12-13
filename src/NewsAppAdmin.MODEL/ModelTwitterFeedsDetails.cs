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

        public double TwitterPageId { get; set; }
        public string FeedText{ get; set; }
        public string FeedLanguage { get; set; }
        public string FeedTextDetail { get; set; }
        public double FeedPostId { get; set; }
        public DateTime FeedPostDate { get; set; }
        public string FeedImageURL { get; set; }
    }
}
