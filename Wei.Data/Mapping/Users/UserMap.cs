using Wei.Core.Domain.Users;

namespace Wei.Data.Mapping.Users
{
    public class UserMap : WeiEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("A_User");
            this.HasKey(c => c.Id);
            this.Property(c => c.OpenId).IsRequired().HasMaxLength(50);
            this.Property(u => u.FirstName).HasMaxLength(10);
            this.Property(u => u.LastName).HasMaxLength(20);
            this.Property(u => u.LoginName).HasMaxLength(50);
            this.Property(u => u.Password).HasMaxLength(200);
            this.Property(u => u.PasswordSalt).HasMaxLength(20);
            this.Property(u => u.Remark).HasMaxLength(2000);

            this.Property(u => u.Phone).HasMaxLength(50);
            this.Property(u => u.QQ).HasMaxLength(50);
            this.Property(u => u.Email).HasMaxLength(200);
            this.Property(u => u.Channel).HasMaxLength(50);
            this.Property(u => u.LastLoginIp).HasMaxLength(20);

            this.HasMany(u => u.UserAttributeList)
                .WithRequired(up => up.User)
                .HasForeignKey(up => up.UserId).WillCascadeOnDelete(true);

            this.HasMany(u => u.Roles)
                .WithMany()
                .Map(m => m.ToTable("A_User_Role_Mapping"));

            this.HasMany(u => u.Departments)
                .WithMany(x=>x.Users)
                .Map(m => m.ToTable("A_User_Department_Mapping"));
            
            //this.HasRequired(u => u.Creator).WithMany().HasForeignKey(x => x.CreatorId);
            //this.HasRequired(u => u.Updater).WithMany().HasForeignKey(x => x.UpdaterId);

            this.Ignore(x => x.UserName);
            this.Ignore(x => x.TagId_List);
        }
    }
}
