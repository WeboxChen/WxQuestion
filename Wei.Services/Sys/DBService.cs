using System.Data.SqlClient;
using System.Linq;
using Wei.Core.Domain.Sys;
using Wei.Data;

namespace Wei.Services.Sys
{
    public class DBService : IDBService
    {
        private readonly IDbContext _dbContext;

        public DBService(IDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public DBTable GetDBTable(string tname)
        {
            SqlParameter param = new SqlParameter("@tname", tname);

            string sql1 = @"select obj.name as 'Name', extp.value as 'Desc'
from sysobjects obj
  left join sys.extended_properties extp on extp.major_id = obj.id and extp.minor_id = 0 and extp.name = 'MS_Description'
where obj.id = OBJECT_ID(@tname) ";
            DBTable dbtable = this._dbContext.SqlQuery<DBTable>(sql1, param).FirstOrDefault();
            if (dbtable != null)
            {
                SqlParameter param2 = new SqlParameter("@tname", tname);
                string sql2 = @"
select col.name as 'Name', CONVERT(int, col.[type]) as 'Type', col.id as 'ObjId', ISNULL(extp.value, '') as 'Desc'
  , Convert(bit, case when scext_edit.value is not null and scext_edit.value='1' then 1 else 0 end) as 'isEdit'
  , Convert(bit, case when scext_show.value is not null and scext_show.value='0' then 0 else 1 end) as 'isShow'
from syscolumns col
  left join sys.extended_properties extp on extp.major_id = col.id and extp.minor_id = col.colid and extp.name = 'MS_Description'

  left join sys.extended_properties scext_edit on col.id=scext_edit.major_id and col.colid=scext_edit.minor_id and scext_edit.name='_edit'
left join sys.extended_properties scext_show on col.id=scext_show.major_id and col.colid=scext_show.minor_id and scext_show.name='_show'
where col.id = OBJECT_ID(@tname)";
                var columnlist = this._dbContext.SqlQuery<DBColumn>(sql2, param2).ToList();

                dbtable.DBColumnList.AddRange(columnlist);
            }
            return dbtable;
        }
    }
}
