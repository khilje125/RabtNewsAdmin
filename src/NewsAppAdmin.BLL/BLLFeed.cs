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
            param[4] = new SqlParameter("@FeedCoverImagePath",objModelFeed.FeedCoverImagePath );
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
    }
}
