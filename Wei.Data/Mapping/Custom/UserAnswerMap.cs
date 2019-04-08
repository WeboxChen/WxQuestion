using Wei.Core.Domain.Custom;

namespace Wei.Data.Mapping.Custom
{
    public class UserAnswerMap: WeiEntityTypeConfiguration<UserAnswer>
    {
        public UserAnswerMap()
        {
            this.ToTable("C_UserAnswer");

            this.HasKey(x => x.Id);

            this.HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.User_Id);
            this.HasRequired(x => x.QuestionBank)
                .WithMany()
                .HasForeignKey(x => x.QuestionBank_Id);
        }
    }
}
