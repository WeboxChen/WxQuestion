using Wei.Core.Domain.Questions;

namespace Wei.Data.Mapping.Questions
{
    public class QuestionBankTagMap: WeiEntityTypeConfiguration<QuestionBankTag>
    {
        public QuestionBankTagMap()
        {
            this.ToTable("Q_QuestionBankTag");

            this.HasKey(x => x.Id);

            this.HasRequired(x => x.ModuleType)
                .WithMany()
                .HasForeignKey(x => x.Type);
        }
    }
}
