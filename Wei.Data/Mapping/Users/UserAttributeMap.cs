using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wei.Core.Domain.Users;

namespace Wei.Data.Mapping.Users
{
    public class UserAttributeMap: WeiEntityTypeConfiguration<UserAttribute>
    {
        public UserAttributeMap()
        {
            this.ToTable("A_UserAttribute");
            this.HasKey(x => x.Id);
        }
    }
}
