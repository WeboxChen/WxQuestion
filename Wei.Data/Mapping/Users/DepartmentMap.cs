using Wei.Core.Domain.Users;

namespace Wei.Data.Mapping.Users
{
    public class DepartmentMap : WeiEntityTypeConfiguration<Department>
    {
        public DepartmentMap()
        {
            this.ToTable("A_Department");
            this.HasKey(c => c.Id);

            this.Property(x => x.Name).HasMaxLength(50);
            this.Property(x => x.Code).HasMaxLength(20);
            this.Property(x => x.Description).HasMaxLength(200);

            //this.HasRequired(x => x.Manager)
            //    .WithMany()
            //    .HasForeignKey(x => x.ManagerId);
            
        }
    }
}
