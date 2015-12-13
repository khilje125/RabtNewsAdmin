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
    public class BLLAdminPages
    {

        #region "Insert Admin Page"

        public decimal InsertAdminPage(ModelAdminPage objModelAdminPage)
        {
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@AdminPageNameEn", objModelAdminPage.AdminPageNameEn);
            param[1] = new SqlParameter("@AdminPageNameAr", objModelAdminPage.AdminPageNameAr);
            param[2] = new SqlParameter("@AdminPageURL", objModelAdminPage.AdminPageURL);
            param[3] = new SqlParameter("@IsActive", objModelAdminPage.IsActive);
            // param[4] = new SqlParameter("@FeedTitlePath", objModelAdminPage.IsShowInMenu);
            param[4] = new SqlParameter("@AdminPageAddedBy", objModelAdminPage.AdminPageAddedBy);

            return DALCommon.ExecuteNonQueryReturnIdentity("sp_Admin_InsertAdminPages", param);

        }

        #endregion

        #region "Get Admin Pages List"

        public List<ModelAdminPage> GetAdminPagesList(int page = 0)
        {
            List<ModelAdminPage> ModelAdminPageList = new List<ModelAdminPage>();
            DataTable dt = new DataTable();
            dt = DALCommon.GetDataByStoredProcedure("sp_Admin_GetAdminPages");


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    ModelAdminPage objModelAdminPage = new ModelAdminPage();
                    objModelAdminPage.AdminPageID = Convert.ToDouble(item["AdminPageID"]);
                    objModelAdminPage.AdminPageNameEn = Convert.ToString(item["AdminPageNameEn"]);
                    objModelAdminPage.AdminPageNameAr = Convert.ToString(item["AdminPageNameAr"]);
                    objModelAdminPage.AdminPageURL = Convert.ToString(item["AdminPageURL"]);
                    objModelAdminPage.IsActive = Convert.ToBoolean(item["IsActive"]);
                    //objModelAdminPage.IsShowInMenu = Convert.ToBoolean(item["AddedDate"]);
                    objModelAdminPage.AdminPageAddedBy = Convert.ToDouble(item["AdminPageAddedBy"]);
                    objModelAdminPage.AddedDate = Convert.ToDateTime(item["AddedDate"]);
                    ModelAdminPageList.Add(objModelAdminPage);

                }
            }
            return ModelAdminPageList;
        }

        #endregion

        #region "Get Admin Pages List"

        public List<ModelLeftMenu> LeftMenu()
        {
            DataTable dt = new DataTable();
            dt = DALCommon.GetDataByStoredProcedure("sp_Admin_GetLeftMenu");

            List<ModelLeftMenu> LeftMenu = new List<ModelLeftMenu>();
            List<ModelLeftMenu> LeftMenuReturn = new List<ModelLeftMenu>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {

                    ModelLeftMenu objModelLeftMenu = new ModelLeftMenu();
                    objModelLeftMenu.Id = Convert.ToInt32(item["MenuID"]);
                    objModelLeftMenu.MenuText = Convert.ToString(item["MenuTitle"]);
                    objModelLeftMenu.URL = Convert.ToString(item["AdminPageURL"]);
                    objModelLeftMenu.ParentId = item["ParentID"] != DBNull.Value ? Convert.ToInt32(item["ParentID"]) : (int?)null;
                    objModelLeftMenu.IsActive = Convert.ToBoolean(item["IsActive"]);
                    LeftMenu.Add(objModelLeftMenu);

                }
                LeftMenuReturn = GetMenuTree(LeftMenu, null);
            }
            return LeftMenuReturn;
        }
        private List<ModelLeftMenu> GetMenuTree(List<ModelLeftMenu> list, int? parentId)
        {
            return list.Where(x => x.ParentId == parentId).Select(x => new ModelLeftMenu()
            {
                Id = x.Id,
                MenuText = x.MenuText,
                URL = x.URL,
                ParentId = x.ParentId,
                IsActive = x.IsActive,
                List = GetMenuTree(list, x.Id)
            }).ToList();

        }
        #endregion

    }
}
