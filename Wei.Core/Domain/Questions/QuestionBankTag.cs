namespace Wei.Core.Domain.Questions
{
    /// <summary>
    /// 题库标签
    /// </summary>
    public class QuestionBankTag: BaseEntity
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标签题库类型
        /// </summary>
        public int Type { get; set; }
        public virtual ModuleType ModuleType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
