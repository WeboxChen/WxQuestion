using Wei.Core.Domain.Custom;

namespace Wei.Data.Mapping.Custom
{
    public class UserAnswerDetailMap
        : WeiEntityTypeConfiguration<UserAnswerDetail>
    {
        public UserAnswerDetailMap()
        {
            this.ToTable("C_UserAnswerDetail");

            this.HasKey(x => x.Id);

            this.HasRequired(x => x.UserAnswer)
                .WithMany(x => x.UserAnswerDetailList)
                .HasForeignKey(x => x.UserAnswer_Id);


        }
    }
}
