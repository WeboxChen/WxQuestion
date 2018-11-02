﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Wei.Core;

namespace Wei.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly DBHelp _dbHelp;
        public CommonService(DBHelp dbHelp)
        {
            this._dbHelp = dbHelp;
        }

        public int Insert(string tblname, IDictionary<string, object> data)
        {
            string sql = " insert into {0} ({1}) values ({2}) ";
            List<SqlParameter> paras = new List<SqlParameter>();
            StringBuilder cols = new StringBuilder();
            StringBuilder vals = new StringBuilder();
            
            foreach(var item in data)
            {
                if (item.Key.Equals("id", StringComparison.CurrentCultureIgnoreCase) || item.Value == null)
                    continue; 

                cols.AppendFormat(" {0} ", item.Key);
                vals.AppendFormat(" @{0} ", item.Key);
                paras.Add(new SqlParameter("@" + item.Key, item.Value));
                
                cols.Append(" , ");
                vals.Append(" , ");
            }
            if (cols.Length > 2)
                cols = cols.Remove(cols.Length - 2, 2);
            if (vals.Length > 2)
                vals = vals.Remove(vals.Length - 2, 2);

            int result = _dbHelp.InsertIdentity(string.Format(sql, tblname, cols.ToString(), vals.ToString()) + "  select @@IDENTITY"
                , System.Data.CommandType.Text, paras.ToArray());
            return result;
        }

        public void Insert(IDictionary<string, IDictionary<string, object>> data)
        {
            StringBuilder sql = new StringBuilder();
            IList<SqlParameter> paras = new List<SqlParameter>();
            foreach(var item in data)
            {
                if (item.Value.Count == 0)
                    continue;
                string tname = item.Key;
                StringBuilder cols = new StringBuilder();
                StringBuilder vals = new StringBuilder();
                foreach(var j in item.Value)
                {
                    if (j.Key.Equals("id", StringComparison.CurrentCultureIgnoreCase) || j.Value == null || string.IsNullOrEmpty(j.Value.ToString()))
                        continue;
                    string parakey = tname + "_" + j.Key;
                    cols.AppendFormat(" {0} ", j.Key);
                    vals.AppendFormat(" @{0} ", parakey);
                    paras.Add(new SqlParameter("@" + parakey, j.Value));
                    
                    cols.Append(" , ");
                    vals.Append(" , ");
                }
                if (cols.Length > 2)
                    cols = cols.Remove(cols.Length - 2, 2);
                if (vals.Length > 2)
                    vals = vals.Remove(vals.Length - 2, 2);
                sql.Append(string.Format(" insert into {0} ({1}) values ({2}) ", tname, cols.ToString(), vals.ToString()));
            }
            _dbHelp.Insert(sql.ToString(), System.Data.CommandType.Text, paras.ToArray());
        }

        /// <summary>
        /// 多项更新
        /// </summary>
        /// <param name="data"></param>
        public void Update(IDictionary<string, IDictionary<string, object>> data)
        {
            StringBuilder sql = new StringBuilder();
            IList<SqlParameter> paras = new List<SqlParameter>();
            foreach (var tblDir in data)
            {
                int i = 1;
                // 更新列小于等于1， 列不正确。 id + 需要更新列， 至少两列
                // 没有Id列，不更新
                if (tblDir.Value.Count <= 1 || !tblDir.Value.Keys.Contains("Id"))
                    continue;
                StringBuilder sb = new StringBuilder(string.Format(" update {0} set ", tblDir.Key));
                foreach (var item in tblDir.Value)
                {
                    if (item.Key.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    string paraname = string.Format("@{0}_{1}", tblDir.Key, item.Key);
                    paras.Add(new SqlParameter(paraname, item.Value));

                    if (i == 1)
                        sb.AppendFormat(" {0}={1} ", item.Key, paraname);
                    else
                        sb.AppendFormat(", {0}={1} ", item.Key, paraname);
                    i++;
                }
                // 添加更新条件
                string idParaname = string.Format("@{0}_{1}", tblDir.Key, "Id");
                sb.AppendFormat(" where Id={0} ", idParaname);
                paras.Add(new SqlParameter(idParaname, tblDir.Value["Id"]));

                sql.Append(sb.ToString());
            }
            _dbHelp.Execute(sql.ToString(), System.Data.CommandType.Text, paras.ToArray());
        }

        /// <summary>
        /// 单表更新
        /// </summary>
        /// <param name="tblname"></param>
        /// <param name="data"></param>
        public void Update(string tblname, IDictionary<string, object> data)
        {
            string sql = " update {0} set {1} {2} ";
            string whereStr = "";
            StringBuilder cols = new StringBuilder();
            IList<SqlParameter> paras = new List<SqlParameter>();
            
            foreach (var item in data)
            {
                if(item.Key.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                {
                    if(whereStr.Length == 0)
                    {
                        whereStr = " where Id=@id ";
                        paras.Add(new SqlParameter("@" + item.Key, item.Value));
                    }
                    continue;
                }
                if (item.Value == null) continue;
                cols.AppendFormat(" {0}=@{0} ", item.Key);
                paras.Add(new SqlParameter("@" + item.Key, item.Value));
                cols.Append(" ,");
            }

            if (whereStr.Length == 0)
                throw new Exception("更新条件错误！未找到id");
            _dbHelp.Execute(string.Format(sql, tblname, cols.ToString().Trim(','), whereStr), CommandType.Text, paras.ToArray());
        }

        /// <summary>
        /// 单表更新
        /// </summary>
        /// <param name="tblname"></param>
        /// <param name="data"></param>
        /// <param name="whereStr"></param>
        public void Update(string tblname, IDictionary<string, object> data, string whereStr)
        {
            if (string.IsNullOrEmpty(whereStr))
                throw new Exception("丢失更新条件！");

            string sql = " update {0} set {1} {2} ";
            StringBuilder cols = new StringBuilder();
            IList<SqlParameter> paras = new List<SqlParameter>();

            int i = 0;
            foreach(var item in data)
            {
                i++;
                cols.AppendFormat(" {0}=@{0} ", item.Key);
                paras.Add(new SqlParameter("@" + item.Key, item.Value));
                if (i == data.Count)
                    break;
                cols.Append(" , ");
            }
            _dbHelp.Execute(string.Format(sql, tblname, cols.ToString(), whereStr), CommandType.Text, paras.ToArray());
        }
        
        public void Delete(IDictionary<string, IDictionary<string, object>> data)
        {
            if(data == null || data.Count ==0)
                throw new Exception("丢失更新条件！");
            string sql = "";
            foreach(var i in data)
            {
                if(i.Value.ContainsKey("Id"))
                    sql += string.Format("delete {0} where id={1}", i.Key, i.Value["Id"]);
            }
            if(sql.Length>0)
                _dbHelp.Execute(sql, System.Data.CommandType.Text);
        }

        public void Delete(string tblname, IDictionary<string, object> data)
        {
            IList<SqlParameter> paras = new List<SqlParameter>();
            string wherestr = "";
            foreach(var j in data)
            {
                if (j.Key.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                {
                    wherestr = " and Id=@id ";
                    paras.Add(new SqlParameter("@" + j.Key, j.Value));
                    break;
                }
                //string parakey = string.Format("@{0}", j.Key);
                //wherestr += string.Format(" and {0}={1} ", j.Key, parakey);
                //paras.Add(new SqlParameter(parakey, j.Value));
            }
            if (wherestr.Length > 0)
                _dbHelp.Execute(string.Format(" delete {0} where 1=1 {1} ", tblname, wherestr), CommandType.Text, paras.ToArray());
        }

        public DataTable GetByProcedure(string procedure, IDictionary<string, object> paras)
        {
            IList<SqlParameter> list = new List<SqlParameter>();
            foreach (var item in paras)
            {
                list.Add(new SqlParameter(item.Key, item.Value));
            }
            var set = _dbHelp.Select(procedure, CommandType.StoredProcedure, list.ToArray());
            if (set != null && set.Tables.Count != 0)
                return set.Tables[0];
            return null;
        }
        public DataTable GetByProcedure(string procedure, IDictionary<string, object> paras, out string msg)
        {
            msg = "";
            IList<SqlParameter> list = new List<SqlParameter>();
            foreach (var item in paras)
            {
                list.Add(new SqlParameter(item.Key, item.Value));
            }
            SqlParameter para = new SqlParameter("@msg", "");
            para.Size = -1;
            para.DbType = DbType.String;
            para.Direction = ParameterDirection.Output;
            list.Add(para);
            var set = _dbHelp.Select(procedure, CommandType.StoredProcedure, list.ToArray());
            msg = para.Value.ToString();
            if (set != null && set.Tables.Count != 0)
                return set.Tables[0];
            return null;
        }
        public DataTable GetByProcedure(string procedure, IDictionary<string, object> paras, out int count, out int fcount)
        {
            count = 0;
            fcount = 0;
            IList<SqlParameter> list = new List<SqlParameter>();
            foreach (var item in paras)
            {
                list.Add(new SqlParameter(item.Key, item.Value));
            }
            var set = _dbHelp.Select(procedure, CommandType.StoredProcedure, list.ToArray());
            if (set != null && set.Tables.Count != 0)
                return set.Tables[0];
            return null;
        }

        public DataTable GetByView(string view, string columns, string filter = "", string sort = "")
        {
            string sql = string.Format(" select Id, {0} from {1} where 1=1 {2} ", columns, view, filter);
            var set = _dbHelp.Select(sql, CommandType.Text);
            if (set != null)
                return set.Tables[0];
            return null;
        }

        public DataTable GetByView(string view, string columns, out int count, out int fcount, int start = 0, int length = 20, string filter = "", string sort = "Id")
        {
            count = 0;
            fcount = 0;
            string sql = string.Format(@"select count(*) from {1};
                select count(*) from {1} where 1=1 {2};
                select * from 
                    (select {3} ROW_NUMBER() over(order by {5}) as 'RowNo', Id, {0} from {1} where 1=1 {2} ) as temp 
                where 1=1 {4} ;  "
                , columns, view, filter
                , length > 0 ? " top " + (start + length) : ""
                , " and RowNo>" + start
                , sort);
            var set = _dbHelp.Select(sql, CommandType.Text);
            if (set != null && set.Tables.Count == 3)
            {
                count = set.Tables[0].Rows[0][0].ToInt();
                fcount = set.Tables[1].Rows[0][0].ToInt();
                return set.Tables[2];
            }
            return null;
        }

        public DataTable GetByViewNonAlias(string view, string columns, out int count, out int fcount, int start = 0, int length = 20, string filter = "", string sort = "Id")
        {
            count = 0;
            fcount = 0;
            string sql = string.Format(@"select count(*) from {1};
                select count(*) from {1} where 1=1 {2};
                select * from 
                    (select {3} ROW_NUMBER() over(order by {5}) as 'RowNo', {0} from {1} where 1=1 {2} ) as temp 
                where 1=1 {4} ;  "
                , columns, view, filter
                , length > 0 ? " top " + (start + length) : ""
                , " and RowNo>" + start
                , sort);
            var set = _dbHelp.Select(sql, CommandType.Text);
            if (set != null && set.Tables.Count == 3)
            {
                count = set.Tables[0].Rows[0][0].ToInt();
                fcount = set.Tables[1].Rows[0][0].ToInt();
                return set.Tables[2];
            }
            return null;
        }



        public int ExecuteProcedure(string procname, IDictionary<string, object> paras)
        {
            IList<SqlParameter> list = new List<SqlParameter>();
            foreach(var item in paras)
            {
                list.Add(new SqlParameter(item.Key, item.Value));
            }
            int result = _dbHelp.Execute(procname, CommandType.StoredProcedure, list.ToArray());
            return result;
        }

        public int ExecuteProcedure(string procname, IDictionary<string, object> paras, out string msg)
        {
            IList<SqlParameter> list = new List<SqlParameter>();
            foreach (var item in paras)
            {
                list.Add(new SqlParameter(item.Key, item.Value));
            }
            SqlParameter para = new SqlParameter("@msg", "");
            para.Size = -1;
            para.DbType = DbType.String;
            para.Direction = ParameterDirection.Output;
            list.Add(para);
            int result = _dbHelp.Execute(procname, CommandType.StoredProcedure, list.ToArray());
            msg = para.Value.ToString();
            return result;
        }

        public IList<string> GetParasByProcName(string procname)
        {
            string sql = "select name from syscolumns where id=object_id(@procname) and isoutparam=0";
            SqlParameter para = new SqlParameter("@procname", procname);
            var set = _dbHelp.Select(sql, CommandType.Text, para);
            DataTable table = set.Tables[0];

            IList<string> list = new List<string>();
            foreach (DataRow row in table.Rows)
                list.Add(row["name"].ToString());
            return list;
        }
    }
}
