using Wei.Core.Domain.Questions;

namespace Wei.Data.Mapping.Questions
{
    public class QuestionBankMap: WeiEntityTypeConfiguration<QuestionBank>
    {
        public QuestionBankMap()
        {
            this.ToTable("Q_QuestionBank");

            this.HasKey(x => x.Id);

            this.HasRequired(x => x.ModuleType)
                .WithMany()
                .HasForeignKey(x => x.Type);
        }
    }
}
