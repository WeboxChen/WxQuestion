using System;
using System.Collections.Generic;

namespace Wei.Core.Domain.Users
{
    public class User:BaseEntity
    {
        private ICollection<Department> _departments;
        private ICollection<Role> _roles;

        public string GId { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string LastName { get; set; }

        public int Gender { get; set; }

        public string Remark { get; set; }

        public string Phone { get; set; }

        public string QQ { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// 注册渠道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        ///// <summary>
        ///// 创建人Id
        ///// </summary>
        //public int? CreatorId { get; set; }
        //public User Creator { get; set; }

        public DateTime? LastLoginTime { get; set; }

        //public DateTime? UpdatedTime { get; set; }

        //public int? UpdaterId { get; set; }
        //public User Updater { get; set; }

        /// <summary>
        /// 最后登录Ip
        /// </summary>
        public string LastLoginIp { get; set; }

        public DateTime LastActivityTime { get; set; }

        /// <summary>
        /// 状态 
        /// -1: 禁用
        /// 0: 未激活
        /// 1: 在使用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否是管理员  1:管理员 0:普通用户， 做快速区别
        /// </summary>
        public int IsAdmin { get; set; }

        /// <summary>
        /// 登录票据
        /// </summary>
        public string LoginTicket { get; set; }

        public ICollection<Department> Departments
        {
            get { return _departments ?? (_departments = new List<Department>()); }
            protected set { _departments = value; }
        }

        public ICollection<Role> Roles
        {
            get { return _roles ?? (_roles = new List<Role>()); }
            protected set { _roles = value; }
        }

        public string UserName
        {
            get
            {
                string username = "";
                if (!string.IsNullOrEmpty(FirstName))
                    username += FirstName;
                if (!string.IsNullOrEmpty(LastName))
                    username += LastName;
                return username;
            }
        }
    }
}
