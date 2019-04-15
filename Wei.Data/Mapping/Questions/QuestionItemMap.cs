using Wei.Core.Domain.Questions;

namespace Wei.Data.Mapping.Questions
{
    public class QuestionItemMap
        : WeiEntityTypeConfiguration<QuestionItem>
    {
        public QuestionItemMap()
        {
            this.ToTable("Q_QuestionItem");
            this.HasKey(c => c.Id);

            this.HasRequired(x => x.Question)
                .WithMany(x => x.QuestionItemList)
                .HasForeignKey(x => x.QuestionId)
                .WillCascadeOnDelete(true);
        }
    }
}
