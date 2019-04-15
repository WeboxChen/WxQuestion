namespace Wei.Core.Domain.Questions
{
    public class QuestionItem : BaseEntity
    {
        /// <summary>
        /// 题目
        /// </summary>
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
        public int Sort { get; set; }

    }
}
