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
        public decimal QuestionNo { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 下一题
        /// </summary>
        public decimal? Next { get; set; }
        /// <summary>
        /// 提出问题时间
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// 回答问题时间
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// 语音路径
        /// </summary>
        public string VoicePath { get; set; }

        /// <summary>
        /// 问题代码
        /// </summary>
        public string QCode { get; set; }
    }
}
