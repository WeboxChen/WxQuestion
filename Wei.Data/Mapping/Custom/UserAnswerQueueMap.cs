using Wei.Core.Domain.Custom;

namespace Wei.Data.Mapping.Custom
{
    public class UserAnswerQueueMap: WeiEntityTypeConfiguration<UserAnswerQueue>
    {
        public UserAnswerQueueMap()
        {
            this.ToTable("C_UserAnswerQueue");

            this.HasKey(x => x.Id);
            this.HasRequired(x => x.QuestionAnswer)
                .WithMany()
                .HasForeignKey(x => x.QuestionAnswer_Id);
            this.HasRequired(x => x.UserAnswer)
                .WithMany(x=>x.UserAnswerQueueList)
                .HasForeignKey(x => x.UserAnswer_Id);
        }

    }
}
