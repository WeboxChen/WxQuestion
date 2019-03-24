using Wei.Core.Domain.Users;

namespace Wei.Data.Mapping.Users
{
    public class UserAttributeValueMap: WeiEntityTypeConfiguration<UserAttributeValue>
    {
        public UserAttributeValueMap()
        {
            this.ToTable("A_UserAttributeValue");
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.User)
                .WithMany(u => u.UserAttributeList)
                .HasForeignKey(ua => ua.UserId);
            this.HasRequired(ua => ua.UserAttribute)
                .WithMany()
                .HasForeignKey(ua => ua.UserAttributeId);
        }
    }
}
