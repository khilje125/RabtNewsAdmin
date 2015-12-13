using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.MODEL
{
    public class ModelTwitterFeeds
    {
        public double FeedId { get; set; }
        public double UserPageId { get; set; }
        public string UserPageTitle { get; set; }
        public string UserScreenName { get; set; }
        public string UserPageDesc { get; set; }
        public string UserPageLanguage { get; set; }
        public double UserPageFollowers { get; set; }
        public string UserPageCoverImageURL { get; set; }
        public string UserPageLogoImage { get; set; }

        public List<ModelTwitterFeedsDetails> TwitterFeedDetails { get; set; }
    }
}
