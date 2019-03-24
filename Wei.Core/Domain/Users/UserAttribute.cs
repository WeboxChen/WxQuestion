namespace Wei.Core.Domain.Users
{
    public class UserAttribute: BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 显示序号
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
