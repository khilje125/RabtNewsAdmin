using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAppAdmin.MODEL;
using System.Data.SqlClient;
using NewsAppAdmin.DAL;
using System.Data;

namespace NewsAppAdmin.BLL
{
    public class BLLFeed
    {

        #region "Insert Feed Data"

        public decimal InsertFeedData(ModelFeed objModelFeed)
        {
            SqlParameter[] param = new SqlParameter[15];

            param[0] = new SqlParameter("@FeedChannelName", objModelFeed.FeedChannelName);
            param[1] = new SqlParameter("@FeedCategory", objModelFeed.FeedCategory);
            param[2] = new SqlParameter("@FeedURL", objModelFeed.FeedURL);
            param[3] = new SqlParameter("@FeedTitlePath", objModelFeed.FeedTitlePath);
            param[4] = new SqlParameter("@FeedCoverImagePath", objModelFeed.FeedCoverImagePath);
            param[5] = new SqlParameter("@FeedShortDescPath", objModelFeed.FeedShortDescPath);
            param[6] = new SqlParameter("@FeedDetailPageURLPath", objModelFeed.FeedDetailPageURLPath);
            param[7] = new SqlParameter("@FeedPubDatePath", objModelFeed.FeedPubDatePath);
            param[8] = new SqlParameter("@FeedImagePath", objModelFeed.FeedImagePath);
            param[9] = new SqlParameter("@FeedDetailPageImagePath", objModelFeed.FeedDetailPageImagePath);
            param[10] = new SqlParameter("@FeedPostDetailsPath", objModelFeed.FeedPostDetailsPath);
            param[11] = new SqlParameter("@FeedCategoryPath", objModelFeed.FeedCategoryPath);
            param[12] = new SqlParameter("@FeedSubCategoryPath", objModelFeed.FeedSubCategoryPath);
            param[13] = new SqlParameter("@FeedAddedBy", Convert.ToDecimal("1"));
            //param[14] = new SqlParameter("@FeedStatus", objModelFeed.FeedStatus);
            param[14] = new SqlParameter("@FeedStatus", 1);

            return DALCommon.ExecuteNonQueryReturnIdentity("sp_Merchant_InsertFeedData", param);

        }

        #endregion

        #region "Insert Feed Data List"

        public List<ModelFeed> GetFeedDataList(int page = 0)
        {
            List<ModelFeed> ModelFeedList = new List<ModelFeed>();
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[1];


            param[0] = new SqlParameter("@FeedId", page);
            dt = DALCommon.GetDataUsingDataTable("[sp_Admin_GetFeedList]", param);


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    ModelFeed objModelFeed = new ModelFeed();
                    objModelFeed.FeedId = Convert.ToDouble(item["FeedId"]);
                    objModelFeed.FeedChannelName = Convert.ToString(item["FeedChannelName"]);
                    objModelFeed.FeedCategory = Convert.ToString(item["FeedCategory"]);
                    objModelFeed.FeedTitlePath = Convert.ToString(item["FeedTitlePath"]);
                    objModelFeed.FeedImagePath = Convert.ToString(item["FeedImagePath"]);
                    objModelFeed.FeedAddedDate = Convert.ToDateTime(item["FeedAddedDate"]);
                    ModelFeedList.Add(objModelFeed);

                }
            }
            return ModelFeedList;
        }

        #endregion

        #region "Get Feeds URLs"
        public DataTable GetFeedUrls(int? FeedTypeId)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@FeedTypeId", FeedTypeId);
            return DALCommon.GetDataUsingDataTable("sp_Admin_GetFeedUrlWithScrepeSetting", param);
        }
        #endregion

        #region "Get Details Scrapper Hosts Helper"
        public List<ModelFeedsDetailScrappingHelper> GetScrapperHostsData()
        {
            ModelFeedsDetailScrappingHelper model = new ModelFeedsDetailScrappingHelper();
            List<ModelFeedsDetailScrappingHelper> lstModel = new List<ModelFeedsDetailScrappingHelper>();
            DataTable dt = new DataTable();
            dt = DALCommon.GetDataByStoredProcedure("[sp_Admin_GetScrapperHostsHelper]");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    model = new ModelFeedsDetailScrappingHelper();
                    model.ScrapeHostId = Convert.ToDouble(item["ScrapeHostId"].ToString());
                    model.HostName = item["HostName"].ToString();
                    model.DetailPagePath = item["DetailPagePath"].ToString();
                    model.DetailPageImagePath = item["DetailPageImagePath"].ToString();
                    lstModel.Add(model);
                }
            }
            return lstModel;
        }

        #endregion

        #region "Insert TwitterFeeds Details"

        public decimal InsertTwitterScrappedData(ModelTwitterFeeds objModelTwitterFeeds)
        {
            decimal TwitterPageId = 0;
            decimal twitterFeedDetailId = 0;
            try
            {
                SqlParameter[] paramTwitterFeeds = new SqlParameter[9];
                paramTwitterFeeds[0] = new SqlParameter("@FeedId", objModelTwitterFeeds.FeedId);
                paramTwitterFeeds[1] = new SqlParameter("@UserPageId", objModelTwitterFeeds.UserPageId);
                paramTwitterFeeds[2] = new SqlParameter("@UserPageTitle", objModelTwitterFeeds.UserPageTitle);
                paramTwitterFeeds[3] = new SqlParameter("@UserScreenName", objModelTwitterFeeds.UserScreenName);
                paramTwitterFeeds[4] = new SqlParameter("@UserPageDesc", objModelTwitterFeeds.UserPageDesc);
                paramTwitterFeeds[5] = new SqlParameter("@UserPageLanguage", objModelTwitterFeeds.UserPageLanguage);
                paramTwitterFeeds[6] = new SqlParameter("@UserPageFollowers", objModelTwitterFeeds.UserPageFollowers);
                paramTwitterFeeds[7] = new SqlParameter("@UserPageCoverImageURL", objModelTwitterFeeds.UserPageCoverImageURL);
                paramTwitterFeeds[8] = new SqlParameter("@UserPageLogoImage", objModelTwitterFeeds.UserPageLogoImage);

                TwitterPageId = DALCommon.ExecuteNonQueryReturnIdentity("[sp_Admin_InsertTwitterFeedsData]", paramTwitterFeeds);

                if (TwitterPageId > 0)
                {
                    if (objModelTwitterFeeds.TwitterFeedDetails.Count > 0)
                    {
                        foreach (ModelTwitterFeedsDetails Tweet in objModelTwitterFeeds.TwitterFeedDetails)
                        {
                            SqlParameter[] paramFeedsDetails = new SqlParameter[7];
                            paramFeedsDetails[0] = new SqlParameter("@TwitterPageId", TwitterPageId);
                            paramFeedsDetails[1] = new SqlParameter("@TweetPostedId", Tweet.FeedPostedtId);
                            paramFeedsDetails[2] = new SqlParameter("@TweetPostedDate", Tweet.FeedPostDate);
                            paramFeedsDetails[3] = new SqlParameter("@TweetFeedDetailPageURL", Tweet.FeedDetailPageURL);
                            paramFeedsDetails[4] = new SqlParameter("@TweetShortText", Tweet.FeedText);
                            paramFeedsDetails[5] = new SqlParameter("@TweetDetailText", Tweet.FeedTextDetail);

                            if (Tweet.FeedMultimediaList != null && Tweet.FeedMultimediaList.Count > 0)
                            {
                                paramFeedsDetails[6] = new SqlParameter("@IsMultiMedia", true);
                            }
                            else
                            {
                                paramFeedsDetails[6] = new SqlParameter("@IsMultiMedia", false);
                            }

                            twitterFeedDetailId = DALCommon.ExecuteNonQueryReturnIdentity("[sp_Admin_InsertTwitterFeedDataDetails]", paramFeedsDetails);

                            if (Tweet.FeedMultimediaList.Count > 0 && twitterFeedDetailId > 0)
                            {
                                foreach (ModelFeedMultimedia multimediaFile in Tweet.FeedMultimediaList)
                                {
                                    SqlParameter[] parammultimediaFile = new SqlParameter[4];
                                    parammultimediaFile[0] = new SqlParameter("@TwitterFeedDetailId", twitterFeedDetailId);
                                    parammultimediaFile[1] = new SqlParameter("@MultiMediaType", multimediaFile.MultiMediaType);
                                    parammultimediaFile[2] = new SqlParameter("@MultiMediaURL", multimediaFile.MultiMediaURL);
                                    parammultimediaFile[3] = new SqlParameter("@MultiMediaExtension", multimediaFile.MultiMediaExtension);

                                    DALCommon.ExecuteNonQueryReturnIdentity("[sp_Admin_InsertTwitterFeedMultiMedia]", parammultimediaFile);
                                }
                            }
                        }
                    }
                }
                return twitterFeedDetailId;
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "BLLFeed, InsertTwitterScrappedData");
            }
            return twitterFeedDetailId;
        }

        #endregion
    }
}
