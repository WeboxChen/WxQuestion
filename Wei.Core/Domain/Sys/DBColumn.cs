namespace Wei.Core.Domain.Sys
{
    public class DBColumn : BaseEntity
    {
        public int ObjId { get; set; }
        public DBTable Table { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Desc { get; set; }
        public string EditType { get; set; }
        public bool IsEdit { get; set; }
        public bool IsShow { get; set; }
    }
}
