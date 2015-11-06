using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.MODEL
{
    public class ModelFeedsData
    {
        public double FeedId { get; set; }
        public string MainTitle { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public string MainCategory{ get; set; }

        public List<ModelFeedDetail> FeedDetail { get; set; }
    }

}
