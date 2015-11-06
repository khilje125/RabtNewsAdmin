using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.MODEL
{
    public class ModelFeed
    {
        public double FeedId { get; set; }
        [Required(ErrorMessage = "Please Enter Channel Name")]
        [Display(Name = "ChannelName")]
        public string FeedChannelName { get; set; }

        [Required(ErrorMessage = "Please Enter Feed Category")]
        [Display(Name = "FeedCategory")]
        public string FeedCategory { get; set; }

        [Display(Name = "FeedURL")]
        public string FeedURL { get; set; }

        [Display(Name = "TitlePath")]
        public string FeedTitlePath { get; set; }

        [Display(Name = "CoverImagePath")]
        public string FeedCoverImagePath { get; set; }

        [Display(Name = "ShortDescPath")]
        public string FeedShortDescPath { get; set; }

        [Display(Name = "DetailPageURLPath")]
        public string FeedDetailPageURLPath { get; set; }

        [Display(Name = "PubDatePath")]
        public string FeedPubDatePath { get; set; }

        [Display(Name = "ImagePath")]
        public string FeedImagePath { get; set; }

        [Display(Name = "DetailPageImagePath")]
        public string FeedDetailPageImagePath { get; set; }

        [Display(Name = "PostDetailsPath")]
        public string FeedPostDetailsPath { get; set; }

        [Display(Name = "CategoryPath")]
        public string FeedCategoryPath { get; set; }

        [Display(Name = "SubCategoryPath")]
        public string FeedSubCategoryPath { get; set; }

        [Display(Name = "AddedDate")]
        public DateTime FeedAddedDate { get; set; }

        [Display(Name = "AddedBy")]
        public double FeedAddedBy { get; set; }

        [Display(Name = "ModifiedDate")]
        public DateTime FeedModifiedDate { get; set; }

        [Display(Name = "ModifiedBy")]
        public double FeedModifiedBy { get; set; }

        [Display(Name = "FeedStatus")]
        public int FeedStatus { get; set; }
    }
}
