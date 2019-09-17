using System;
using System.Collections.Generic;
using Wei.Core.Domain.Users;

namespace Wei.Core.Domain.Questions
{
    /// <summary>
    /// 题库
    /// </summary>
    public class QuestionBank: BaseEntity
    {
        private ICollection<Question> _questionList;
        private ICollection<UserAttribute> _userAttributeList;

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
        /// 自动响应
        /// </summary>
        public bool? AutoResponse { get; set; }
        /// <summary>
        /// 自动响应语句，多条用“|”隔开
        /// </summary>
        public string ResponseKeyWords { get; set; }
        /// <summary>
        /// 状态 -1：禁用 0：启用 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 有效起始日期
        /// </summary>
        public DateTime? ExpireDateBegin { get; set; }
        /// <summary>
        /// 有效结束日期
        /// </summary>
        public DateTime? ExpireDateEnd { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatorId { get; set; }
        public virtual Users.User Creator { get; set; }

        /// <summary>
        /// 题目集合
        /// </summary>
        public virtual ICollection<Question> QuestionList
        {
            get { return _questionList ?? (_questionList = new List<Question>()); }
            set { _questionList = value; }
        }
        /// <summary>
        /// 题库关联用户属性
        /// </summary>
        public virtual ICollection<UserAttribute> UserAttributeList
        {
            get { return _userAttributeList ?? (_userAttributeList = new List<UserAttribute>()); }
            set { _userAttributeList = value; }
        }
    }
}
