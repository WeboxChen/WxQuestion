using System.Collections.Generic;

namespace Wei.Core.Domain.Sys
{
    public class DBTable : BaseEntity
    {
        private List<DBColumn> _dbcolumnList;

        public string Name { get; set; }
        public string Desc { get; set; }

        public virtual List<DBColumn> DBColumnList
        {
            get
            {
                return _dbcolumnList ?? (_dbcolumnList = new List<DBColumn>());
            }
        }
    }
}
