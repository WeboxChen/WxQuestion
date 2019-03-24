namespace Wei.Core.Domain.Questions
{
    public class QuestionAnswer: BaseEntity
    {
        /// <summary>
        /// 问题Id
        /// </summary>
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        /// <summary>
        /// 答案关键字
        /// </summary>
        public string AnswerKeys { get; set; }
        /// <summary>
        /// 答案描述
        /// </summary>
        public string AnswerDesc { get; set; }
        /// <summary>
        /// 下一题题号
        /// </summary>
        public decimal? Next { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public decimal Sort { get; set; }
    }
}
