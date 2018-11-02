using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wei.Core.Caching;
using Wei.Core.Infrastructure;

namespace Wei.Core
{
    public static class DBStructCaching
    {
        private static readonly string DBCACHETABLES = "DB_CACHE_TABLES";

        private static string GetViewName(int num)
        {
            int first = num / 26 + 97;
            int second = 97 + num % 26;
            return new string(new char[]
            {
                (char)first,
                (char)second
            });
        }

        public static void RegisterDBTables()
        {
            var cache = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("wei_cache_static");
            // a:97   z:122
            if (!cache.IsSet(DBStructCaching.DBCACHETABLES))
            {
                DBHelp help = new DBHelp();
                string sql1 = " select name, id from sysobjects where type='u' ";
                string sql2 = " select name, id, xusertype from syscolumns where id in (select id from sysobjects where type='u') ";
                DataSet set = help.Select(sql1 + sql2, CommandType.Text);
                ICollection<DBTblModel> list = new List<DBTblModel>();
                if(set != null && set.Tables.Count > 0)
                {
                    DataTable table = set.Tables[0];
                    DataTable coltbl = set.Tables[1];
                    for (int i=0;i< table.Rows.Count;i++)
                    {
                        DataRow row = table.Rows[0];
                        DBTblModel tblModel = new DBTblModel()
                        {
                            Name = row["name"].ToString(),
                            TId = row["id"].ToInt(),
                            ViewName = GetViewName(i)
                        };
                        int j = 0;
                        foreach(DataRow colRow in coltbl.Rows)
                        {
                            if(colRow["id"].ToInt() == tblModel.TId)
                            {
                                DBColModel colModel = new DBColModel()
                                {
                                    Name = colRow["name"].ToString(),
                                    Type = colRow["xusertype"].ToInt(),
                                    ViewName = GetViewName(j)
                                };
                                tblModel.Columns.Add(colModel);
                                j++;
                            }
                        }
                        list.Add(tblModel);
                    }
                }
                cache.Set(DBStructCaching.DBCACHETABLES, list, int.MaxValue);
            }
        }

        public static void GetDBName(string viewName, out string tblname, out string colname)
        {
            if (viewName.Length != 5 || viewName.IndexOf('_') != 2)
                throw new ArgumentException("modelName");
            string[] modelarr = viewName.Split('_');
            tblname = "";
            colname = "";
            var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("wei_cache_static");
            var tblStructCatch = cacheManager.Get<ICollection<DBTblModel>>(DBStructCaching.DBCACHETABLES);
            DBTblModel model = tblStructCatch.First(x => x.ViewName == modelarr[0]);
            
            tblname = model.Name;
            colname = model.Columns.First(x => x.ViewName == modelarr[1]).Name;
        }

        public static string GetViewName(string tblname, string colname)
        {
            if (string.IsNullOrEmpty(tblname) || string.IsNullOrEmpty(colname))
                throw new ArgumentException("tblname or colname");

            var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("wei_cache_static");
            var tblStructCatch = cacheManager.Get<ICollection<DBTblModel>>(DBStructCaching.DBCACHETABLES);
            DBTblModel model = tblStructCatch.First(x => x.Name == tblname);

            string result = model.ViewName;
            result += model.Columns.First(x => x.Name == colname).ViewName;
            return result;
        }
    }

    public class DBTblModel
    {
        public DBTblModel()
        {
            Columns = new List<DBColModel>();
        }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 前端显示名
        /// </summary>
        public string ViewName { get; set; }
        public int TId { get; set; }
        public ICollection<DBColModel> Columns { get; set; }
    }
    public class DBColModel
    {
        public string Name { get; set; }
        public string ViewName { get; set; }
        public int Type { get; set; }
    }
}
