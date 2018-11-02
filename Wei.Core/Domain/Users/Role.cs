using System.Collections.Generic;

namespace Wei.Core.Domain.Users
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class Role : BaseEntity
    {
        //private ICollection<User> _users;
        private ICollection<PermissionRecord> _permissionRecords;
        

        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int Sort { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// 角色权限信息
        /// </summary>
        public ICollection<PermissionRecord> PermissionRecords
        {
            get { return _permissionRecords ?? (_permissionRecords = new List<PermissionRecord>()); }
            protected set { _permissionRecords = value; }
        }

        ///// <summary>
        ///// 拥有改角色的用户信息
        ///// </summary>
        //public ICollection<User> Users
        //{
        //    get { return _users ?? (_users = new List<User>()); }
        //    protected set { _users = value; }
        //}
    }
}
