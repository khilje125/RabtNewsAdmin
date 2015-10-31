using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NewsAppAdmin.DAL
{

    public class DALCommon
    {
        public SqlCommand _sqlCommand = null;
        public SqlDataAdapter _sqlDataAdapter = null;
        public SqlDataReader _sqlDataReader = null;
        public DataTable _dataTableRecord = null;
        public int _intRowsAffected = 0;
        public decimal _countRecord = 0;
        public decimal _DecimalIdentity = 0;

        #region "Define Connection String"
        private static string ConnectionString()
        {
            string DbCon = Convert.ToString(ConfigurationManager.ConnectionStrings["RabtAdminDbCon"]);
            // ConfigurationManager.ConnectionStrings("RabtAdminDbCon").ConnectionString;
            return DbCon;
        }
        #endregion

        #region "Execute Non Query"
        public static int ExecuteNonQuery(string spName, SqlParameter[] paramtr)
        {
            int recCount = 0;
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                if ((cn.State == ConnectionState.Closed))
                {
                    cn.Open();
                }

                recCount = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, ExecuteNonQuery");
            }
            finally
            {
                cmd = null;
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }
            }

            return recCount;
        }

        public static int ExecuteNonQueryByQuery(string Query, SqlParameter[] paramtr)
        {
            int recCount = 0;
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Query;
            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                if ((cn.State == ConnectionState.Closed))
                {
                    cn.Open();
                }
                recCount = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, ExecuteNonQueryByQuery");
            }
            finally
            {
                cmd = null;
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }
            }
            return recCount;
        }

        public static decimal ExecuteNonQueryReturnIdentity(string spName, SqlParameter[] paramtr)
        {
            decimal identity = 0;
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            //For returning Value 
            SqlParameter returnValue = default(SqlParameter);
            returnValue = cmd.Parameters.Add("@num", System.Data.SqlDbType.Real);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

            try
            {
                if ((cn.State == ConnectionState.Closed))
                {
                    cn.Open();
                }

                cmd.ExecuteNonQuery();

                identity = Convert.ToDecimal(cmd.Parameters["@num"].Value);

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, ExecuteNonQueryReturnIdentity");
            }
            finally
            {
                cmd = null;
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }
            }
            return identity;
        }

        public static int ExecuteTransaction(string spName, SqlCommand cmd, SqlParameter[] paramtr)
        {
            int intCount = 0;

            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                intCount = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, ExecuteTransaction");
            }

            return intCount;
        }
        #endregion

        #region "Get Data Using Data Table"
        public static DataTable GetDataUsingDataTable(string spName, SqlParameter[] paramtr)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, GetDataUsingDataTable");
            }

            return dt;
        }

        public static DataTable GetDataByQuery(string Query, SqlParameter[] paramtr)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Query;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, GetDataUsingDataTableByQuery");
            }

            return dt;
        }

        public static DataTable GetDataByQuery(string Query)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Query;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, GetDataUsingDataTableByQuery");
            }

            return dt;
        }

        public static DataTable GetDataByStoredProcedure(string spName)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            try
            {
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, GetDataUsingDataTable");
            }

            return dt;
        }

        public static DataTable GetDataByParameter(string spName, string ParameterName, object ParameterValue)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue(ParameterName, ParameterValue);

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, GetDataUsingDataTableByParameter");
            }

            return dt;
        }

        public static int GetCountByProcedure(string spName, SqlParameter[] paramtr)
        {
            SqlDataReader dr = null;
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            int result = 0;
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                if ((cn.State == ConnectionState.Closed))
                {
                    cn.Open();
                }

                dr = cmd.ExecuteReader();

                if ((dr.HasRows))
                {
                    while (dr.Read())
                    {
                        result = Convert.ToInt32(dr.GetValue(0));
                    }
                }
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, GetCountByProcedure");
            }
            finally
            {
                cmd = null;
                dr = null;
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }
            }

            return result;
        }
        #endregion

        #region "Data Exists"
        public static bool DataExistsByStoredProcedure(string spName, SqlParameter[] paramtr)
        {
            SqlDataReader dr = null;
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                if ((cn.State == ConnectionState.Closed))
                {
                    cn.Open();
                }

                dr = cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, DataExistsByStoredProcedure");
            }

            if ((dr.HasRows))
            {
                dr = null;
                cmd = null;
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }

                return true;
            }
            else
            {
                dr = null;
                cmd = null;
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }

                return false;
            }
        }

        public static int DataExistIntByStoredProcedure(string spName, SqlParameter[] paramtr)
        {
            SqlDataReader dr = null;
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            int j = 0;

            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                if ((cn.State == ConnectionState.Closed))
                {
                    cn.Open();
                }

                dr = cmd.ExecuteReader();

                if ((dr.HasRows))
                {
                    while (dr.Read())
                    {
                        j = Convert.ToInt32(dr.GetValue(0));
                    }
                }

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, DataExistIntByStoredProcedure");
            }
            finally
            {
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }
                dr = null;
                cmd = null;
            }

            return j;
        }

        public static decimal GetIDByStoredProcedure(string spName, SqlParameter[] paramtr)
        {
            SqlDataReader dr = null;
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            decimal j = 0;

            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                if ((cn.State == ConnectionState.Closed))
                {
                    cn.Open();
                }

                dr = cmd.ExecuteReader();

                if ((dr.HasRows))
                {
                    while (dr.Read())
                    {
                        j = Convert.ToDecimal(dr.GetValue(0));
                    }
                }
                else
                {
                    j = 0;
                }

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, DataExistDblByStoredProcedure");
            }
            finally
            {
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }
                dr = null;
                cmd = null;
            }

            return j;
        }

        public static string DataExistStringByStoredProcedure(string spName, SqlParameter[] paramtr)
        {
            SqlDataReader dr = null;
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            string j = string.Empty;

            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                if ((cn.State == ConnectionState.Closed))
                {
                    cn.Open();
                }

                dr = cmd.ExecuteReader();

                if ((dr.HasRows))
                {
                    while (dr.Read())
                    {
                        j = dr.GetValue(0).ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, DataExistStringByStoredProcedure");
            }
            finally
            {
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }
                dr = null;
                cmd = null;
            }

            return j;
        }

        public static bool DataExistsByQuery(string Query, SqlParameter[] paramtr)
        {
            SqlDataReader dr = null;
            SqlConnection cn = new SqlConnection(ConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Query;

            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }

            try
            {
                if ((cn.State == ConnectionState.Closed))
                {
                    cn.Open();
                }

                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, DataExistsByQuery");
            }

            if ((dr.HasRows))
            {
                dr = null;
                cmd = null;
                if ((cn.State == ConnectionState.Open))
                {
                    cn.Close();
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region "Transactions"
        public static int ExecuteTransactionReturnIdentity(string spName, SqlCommand cmd, SqlParameter[] paramtr)
        {
            int intCount = 0;
            int identity = 0;
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            for (int i = 0; i <= paramtr.Length - 1; i++)
            {
                cmd.Parameters.Add(paramtr[i]);
            }
            //For returning Value 
            SqlParameter returnValue = default(SqlParameter);
            returnValue = cmd.Parameters.Add("@num", System.Data.SqlDbType.Real);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

            try
            {
                cmd.ExecuteNonQuery();
                identity = Convert.ToInt32(cmd.Parameters["@num"].Value);
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "DALCommon.vb, ExecuteTransaction");
            }

            return identity;
        }
        #endregion
    }

}
