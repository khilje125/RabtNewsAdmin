using NewsAppAdmin.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.BLL
{
    public class BLLAdminUser
    {
        #region "Insert And Update Admin Web Permission"
        public bool SaveAdminWebFormPermission(int AdminUserNo, Array ArrayOfPermission)
        {
            SqlParameter[] param = new SqlParameter[5];
            
            //Now Go For One By One Sector
            for (int i = 0; i <= (ArrayOfPermission.Length / 4) - 1; i++)
            {
                param[0] = new SqlParameter("@AdminUserNo", AdminUserNo);
                param[1] = new SqlParameter("@AdminUserWebFormNo", ArrayOfPermission.GetValue(i, 0));
                param[2] = new SqlParameter("@HaveAddPermission", ArrayOfPermission.GetValue(i, 1));
                param[3] = new SqlParameter("@HaveEditPermission", ArrayOfPermission.GetValue(i, 2));
                param[4] = new SqlParameter("@HaveSearchPermission", ArrayOfPermission.GetValue(i, 3));

                //Manage Permission
                DALCommon.ExecuteNonQuery("sp_Admin_UpdateAdminPermission", param);
            }

            return true;
        }
        #endregion

        #region "Get Admin User Details"
        public DataTable GetAdminUserDetailsByLogin(string AdminUserName, string AdminUserPassword)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@AdminUserName", AdminUserName);
            //param[1] = new SqlParameter("@AdminUserPassword", EncryptDecrypt.Encrypt(AdminUserPassword));
            param[1] = new SqlParameter("@AdminUserPassword", AdminUserPassword);
            return DALCommon.GetDataUsingDataTable("sp_Admin_CheckAdminLogin", param);
        }
        #endregion

        #region "Get Admin User Details"
        public DataTable GetAdminUserDetailsByEmail(string AdminEmail)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Email", AdminEmail);

            return DALCommon.GetDataUsingDataTable("sp_Admin_GetAdminUserByEmail", param);
        }
        #endregion

        #region "Get Admin User Details"
        public DataTable GetAdminUserDetailsByUserNo(double AdminUserNo)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@AdminUserNo", AdminUserNo);

            return DALCommon.GetDataUsingDataTable("[sp_Admin_GetAdminDetailsByAdminUserNo]", param);
        }
        #endregion

        #region "Update Password"
        public int UpdateAdminPassword(double AdminUserNo, string Password)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@AdminUserNo", AdminUserNo);
            param[1] = new SqlParameter("@AdminUserPassword", Password);

            return DALCommon.ExecuteNonQuery("[sp_Admin_UpdateAdminUserPassword]", param);
        }
        #endregion

    }
}
