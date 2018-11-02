using System;

namespace Wei.Core.Domain.Users
{
    public class LoginPermission : BaseEntity
    {
        public string Session { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public int IsEffectived { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime AccessTime { get; set; }

    }
}
