using System;

namespace Wei.Core.Domain.Custom
{
    public class UserAnswerDetail: BaseEntity
    {
        /// <summary>
        /// id
        /// </summary>
        public int UserAnswer_Id { get; set; }
        public virtual UserAnswer UserAnswer { get; set; }
        /// <summary>
        /// 题目编号
        /// </summary>
        public int QuestionNo { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 下一题
        /// </summary>
        public int? Next { get; set; }
        /// <summary>
        /// 提出问题时间
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// 回答问题时间
        /// </summary>
        public DateTime? End { get; set; }
    }
}
