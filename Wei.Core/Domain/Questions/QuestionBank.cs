using System;
using System.Collections.Generic;

namespace Wei.Core.Domain.Questions
{
    /// <summary>
    /// 题库
    /// </summary>
    public class QuestionBank: BaseEntity
    {
        private List<Question> _questionList;

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        public virtual ModuleType ModuleType { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态 -1：删除 0：禁用 1：启用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// 题目集合
        /// </summary>
        public virtual List<Question> QuestionList
        {
            get { return _questionList ?? (_questionList = new List<Question>()); }
            set { _questionList = value; }
        }
    }
}
