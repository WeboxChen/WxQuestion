namespace Wei.Core.Domain.Users
{
    public class UserAttributeValue : BaseEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int UserAttributeId { get; set; }

        public virtual UserAttribute UserAttribute { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
