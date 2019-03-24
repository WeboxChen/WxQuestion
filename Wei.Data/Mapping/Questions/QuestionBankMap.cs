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

            this.HasMany(x => x.QuestionList)
                .WithRequired(x => x.QuestionBank)
                .HasForeignKey(x => x.QuestionBank_Id);
            this.HasMany(x => x.UserAttributeList)
                .WithMany()
                .Map(m => m.ToTable("Q_QuestionBank_UserAttribute_Mapping").MapLeftKey("QuestionBankId").MapRightKey("UserAttributeId"));
        }
    }
}
