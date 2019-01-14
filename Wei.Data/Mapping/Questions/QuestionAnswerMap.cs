using Wei.Core.Domain.Questions;

namespace Wei.Data.Mapping.Questions
{
    public class QuestionAnswerMap 
        : WeiEntityTypeConfiguration<QuestionAnswer>
    {
        public QuestionAnswerMap()
        {
            this.ToTable("Q_QuestionAnswer");
            this.HasKey(c => c.Id);

            this.HasRequired(x => x.Question)
                .WithMany(x => x.QuestionAnswerList)
                .HasForeignKey(x => x.QuestionId);
        }
    }
}
