using Wei.Core.Domain.Users;

namespace Wei.Data.Mapping.Users
{
    public class RoleMap : WeiEntityTypeConfiguration<Role>
    {
        public RoleMap()
        {

            this.ToTable("A_Role");
            this.HasKey(c => c.Id);
            
            this.Property(x => x.Name).HasMaxLength(50);
            this.Property(x => x.Code).HasMaxLength(20);
            this.Property(x => x.Description).HasMaxLength(200);
            
            this.HasMany(u => u.PermissionRecords)
                .WithMany(x=>x.Roles)
                .Map(m => m.ToTable("A_Role_PermissionRecord_Mapping"));
        }
    }
}
