using Wei.Core.Domain.Questions;

namespace Wei.Data.Mapping.Questions
{
    public class QuestionMap: WeiEntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            this.ToTable("Q_Question");

            this.HasKey(x => x.Id);

            this.Ignore(x => x.AnswerType);
            this.Ignore(x => x.QuestionType);
            this.Ignore(x => x.MatchingType);
            this.Ignore(x => x.QuestionText);

            this.HasRequired(q => q.QuestionBank)
                .WithMany(q => q.QuestionList)
                .HasForeignKey(q => q.QuestionBank_Id);
        }
    }
}
