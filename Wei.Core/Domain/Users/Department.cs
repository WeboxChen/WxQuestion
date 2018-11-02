using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wei.Core.Domain.Users
{
    public class Department : BaseEntity
    {
        private ICollection<User> _users;

        public string Name { get; set; }

        public string Code { get; set; }

        //public int ManagerId { get; set; }
        //public User Manager { get; set; }

        public string Description { get; set; }

        public int Sort { get; set; }

        public int Status { get; set; }

        public ICollection<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            protected set { _users = value; }
        }
    }
}
