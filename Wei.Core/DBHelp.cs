using System.Data;
using System.Data.SqlClient;

namespace Wei.Core
{
    public class DBHelp
    {
        // public string conString = System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString;
        private string conString;
        public DBHelp(string conn = null)
        {
            if (string.IsNullOrEmpty(conn))
            {
                conn = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
            conString = conn;
        }

        #region Sql、存储过程 查询方法 Select(string commText, CommandType commType, params SqlParameter[] paras)
        /// <summary>
        /// Sql、存储过程 查询方法
        /// </summary>
        /// <param name="commText">sql语句或要执行的存储过程</param>
        /// <param name="commType">执行操作类型</param>
        /// <param name="paras">执行参数列表</param>
        /// <returns></returns>
        public DataSet Select(string commText, CommandType commType, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.CommandText = commText;//设置执行语句
                    comm.Connection = conn;
                    comm.CommandType = commType;//设置执行类型
                    if (paras != null && paras.Length != 0)//判断参数是否为空
                        comm.Parameters.AddRange(paras);//设置执行参数
                    DataSet set = new DataSet();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comm))
                    {
                        conn.Open();
                        adapter.Fill(set);
                        return set;
                    }
                }
            }
        }
        #endregion

        #region 增删改操作 Execute(string commText, CommandType commType, params SqlParameter[] paras)
        /// <summary>
        /// 增删改操作
        /// </summary>
        /// <param name="commText"></param>
        /// <param name="commType"></param>
        /// <param name="paras"></param>
        /// <returns>受影响的行数</returns>
        public int Execute(string commText, CommandType commType, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.CommandText = commText;
                    comm.Connection = conn;
                    comm.CommandType = commType;
                    if (paras != null && paras.Length != 0)
                        comm.Parameters.AddRange(paras);

                    conn.Open();
                    return comm.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region 执行增加操作 Insert(string commText, CommandType commType, params SqlParameter[] paras)
        /// <summary>
        /// 执行增加操作
        /// </summary>
        /// <param name="commText"></param>
        /// <param name="commType"></param>
        /// <param name="paras"></param>
        /// <returns>id</returns>
        public int InsertIdentity(string commText, CommandType commType, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.CommandText = commText;
                    comm.Connection = conn;
                    comm.CommandType = commType;
                    if (paras != null && paras.Length != 0)
                        comm.Parameters.AddRange(paras);

                    conn.Open();
                    return int.Parse(comm.ExecuteScalar().ToString());
                }
            }
        }
        /// <summary>
        /// 执行增加操作
        /// </summary>
        /// <param name="commText"></param>
        /// <param name="commType"></param>
        /// <param name="paras"></param>
        /// <returns>id</returns>
        public void Insert(string commText, CommandType commType, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.CommandText = commText;
                    comm.Connection = conn;
                    comm.CommandType = commType;
                    if (paras != null && paras.Length != 0)
                        comm.Parameters.AddRange(paras);

                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region 执行带事务的sql语句 ExecuteTran(string commText, CommandType commType, params SqlParameter[] paras)
        /// <summary>
        /// 执行带事务的sql语句
        /// </summary>
        /// <param name="commText"></param>
        /// <param name="commType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public int ExecuteTran(string commText, CommandType commType, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    SqlTransaction t = null;
                    int result = 0;
                    comm.CommandText = commText;
                    comm.Connection = conn;
                    comm.CommandType = commType;
                    if (paras != null && paras.Length != 0)
                        comm.Parameters.AddRange(paras);
                    try
                    {
                        conn.Open();
                        t = conn.BeginTransaction();
                        comm.Transaction = t;
                        result = comm.ExecuteNonQuery();
                        t.Commit();
                    }
                    catch
                    {
                        t.Rollback();
                    }
                    finally
                    {
                        t.Dispose();
                    }
                    return result;
                }
            }
        }
        #endregion
    }
}
