using Wei.Core.Domain.Questions;

namespace Wei.Data.Mapping.Questions
{
    public class ModuleTypeMap: WeiEntityTypeConfiguration<ModuleType>
    {
        public ModuleTypeMap()
        {
            this.ToTable("Q_ModuleType");

            this.HasKey(x => x.Id);
        }

    }
}
