using Wei.Core.Domain.Questions;

namespace Wei.Core.Domain.Custom
{
    public class UserAnswerQueue: BaseEntity
    {
        /// <summary>
        /// 用户答题id
        /// </summary>
        public int UserAnswer_Id { get; set; }
        public virtual UserAnswer UserAnswer { get; set; }

        /// <summary>
        /// 题目答案id
        /// </summary>
        public int QuestionAnswer_Id { get; set; }
        public virtual QuestionAnswer QuestionAnswer { get; set; }

        /// <summary>
        /// 队列状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string QCode { get; set; }
    }
}
