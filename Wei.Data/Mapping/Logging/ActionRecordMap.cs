using Wei.Core.Domain.Logging;

namespace Wei.Data.Mapping.Logging
{
    public class ActionRecordMap : WeiEntityTypeConfiguration<ActionRecord>
    {
        public ActionRecordMap()
        {
            this.ToTable("L_ActionRecord");
            this.HasKey(l => l.Id);
            
        }
    }
}
