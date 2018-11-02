using Wei.Core.Domain.Users;

namespace Wei.Data.Mapping.Users
{
    public class PermissionRecordMap : WeiEntityTypeConfiguration<PermissionRecord>
    {
        public PermissionRecordMap()
        {
            this.ToTable("A_PermissionRecord");
            this.HasKey(c => c.Id);

            this.Property(x => x.Name).HasMaxLength(20);
            this.Property(x => x.Code).HasMaxLength(20);
            this.Property(x => x.Route).HasMaxLength(200);

            this.HasMany(u => u.Roles)
                .WithMany()
                .Map(m => m.ToTable("A_Role_PermissionRecord_Mapping"));
        }
    }
}
