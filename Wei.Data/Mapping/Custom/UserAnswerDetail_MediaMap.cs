using Wei.Core.Domain.Custom;

namespace Wei.Data.Mapping.Custom
{
    public class UserAnswerDetail_MediaMap 
        : WeiEntityTypeConfiguration<UserAnswerDetail_Media>
    {
        public UserAnswerDetail_MediaMap()
        {
            this.ToTable("C_UserAnswerDetail_Media");

            this.HasKey(x => x.Id);

            this.Ignore(x => x.MediaType);

            this.HasRequired(x => x.UserAnswerDetail)
                .WithMany(x => x.UserAnswerDetailMediaList)
                .HasForeignKey(x => x.UserAnswerDetail_Id);
        }
    }
}
