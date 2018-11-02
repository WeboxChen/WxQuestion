using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wei.Core.Domain.Users;

namespace Wei.Data.Mapping.Users
{
    public class LoginPermissionMap : WeiEntityTypeConfiguration<LoginPermission>
    {
        public LoginPermissionMap()
        {
            this.ToTable("A_LoginPermission");
            this.HasKey(c => c.Id);
            this.Property(x => x.Session).HasMaxLength(50);
            this.HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
