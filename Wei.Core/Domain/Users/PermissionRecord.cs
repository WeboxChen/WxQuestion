using System.Collections.Generic;

namespace Wei.Core.Domain.Users
{
    /// <summary>
    /// 权限
    /// </summary>
    public class PermissionRecord : BaseEntity
    {
        private ICollection<Role> _roles;

        public string Name { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 权限深度
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 包含改权限的所有角色信息
        /// </summary>
        public ICollection<Role> Roles
        {
            get { return _roles ?? (_roles = new List<Role>()); }
            protected set { _roles = value; }
        }
    }
}
