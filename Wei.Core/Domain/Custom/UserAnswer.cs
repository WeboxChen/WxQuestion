using System;
using System.Collections.Generic;
using Wei.Core.Domain.Questions;

namespace Wei.Core.Domain.Custom
{
    public class UserAnswer: BaseEntity
    {
        private List<UserAnswerDetail> _userAnswerDetailList;
        /// <summary>
        /// 用户id
        /// </summary>
        public int User_Id { get; set; }
        public virtual Users.User User { get; set; }
        /// <summary>
        /// 题库Id
        /// </summary>
        public int QuestionBank_Id { get; set; }
        public virtual QuestionBank QuestionBank { get; set; }
        /// <summary>
        /// 开始答题时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束答题时间
        /// </summary>
        public DateTime? CompletedTime { get; set; }

        /// <summary>
        /// 本轮试题答案
        /// </summary>
        public virtual List<UserAnswerDetail> UserAnswerDetailList
        {
            get { return _userAnswerDetailList ?? (_userAnswerDetailList = new List<UserAnswerDetail>()); }
            set { _userAnswerDetailList = value; }
        }
    }
}
