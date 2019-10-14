using System;
using System.Collections.Generic;
using Wei.Core.Domain.Questions;

namespace Wei.Core.Domain.Custom
{
    /// <summary>
    /// 用户答题
    /// </summary>
    public class UserAnswer: BaseEntity
    {
        private ICollection<UserAnswerDetail> _userAnswerDetailList;
        private ICollection<UserAnswerQueue> _userAnswerQueueList;
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
        /// 状态，  0：正常， -1：作废， 2： 挂起待作废， 1： 完成
        /// </summary>
        public int Status { get; set; }
        public UserAnswerStatus UserAnswerStatus
        {
            get { return (UserAnswerStatus)Status; }
            set { Status = (int)value; }
        }

        /// <summary>
        /// 本轮试题答案
        /// </summary>
        public virtual ICollection<UserAnswerDetail> UserAnswerDetailList
        {
            get { return _userAnswerDetailList ?? (_userAnswerDetailList = new List<UserAnswerDetail>()); }
            set { _userAnswerDetailList = value; }
        }

        /// <summary>
        /// 本次答题所有队列
        /// </summary>
        public virtual ICollection<UserAnswerQueue> UserAnswerQueueList
        {
            get { return _userAnswerQueueList ?? (_userAnswerQueueList = new List<UserAnswerQueue>()); }
            set { _userAnswerQueueList = value; }
        }
    }
}
