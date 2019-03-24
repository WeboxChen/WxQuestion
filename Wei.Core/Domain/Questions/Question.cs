using System;
using System.Collections.Generic;

namespace Wei.Core.Domain.Questions
{
    public class Question: BaseEntity
    {
        private IList<QuestionAnswer> _questionAnswerList;

        /// <summary>
        /// 题库
        /// </summary>
        public int QuestionBank_Id { get; set; }
        public virtual QuestionBank QuestionBank { get; set; }
        /// <summary>
        /// 问题答案类型
        /// 单选SingleCheck、多选MultiCheck、简答Text
        /// </summary>
        public string AType { get; set; }
        public AnswerType AnswerType
        {
            get
            {
                AnswerType atype;
                if (Enum.TryParse<AnswerType>(AType, out atype))
                    return atype;
                return AnswerType.Text;
            }
            set
            {
                AType = value.ToString();
            }
        }
        /// <summary>
        /// 问题表述类型
        /// 文本Text、图片Image
        /// </summary>
        public string QType { get; set; }
        public QuestionType QuestionType
        {
            get
            {
                QuestionType qtype;
                if (Enum.TryParse<QuestionType>(QType, out qtype))
                    return qtype;
                return QuestionType.Text;
            }
            set
            {
                QType = value.ToString();
            }
        }
        /// <summary>
        /// 问题文本内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 图片上传资源Code
        /// </summary>
        public string ImageCode { get; set; }
        /// <summary>
        /// 答案
        /// 单选【a】、多选【a,b,c】、简答【key1,key2,key3】
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 答对后下一题
        /// </summary>
        public decimal? Next1 { get; set; }
        /// <summary>
        /// 答错后下一题
        /// </summary>
        public decimal? Next2 { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public decimal Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 问题答案
        /// </summary>
        public virtual IList<QuestionAnswer> QuestionAnswerList
        {
            get { return _questionAnswerList ?? (_questionAnswerList = new List<QuestionAnswer>()); }
            set { _questionAnswerList = value; }
        }
    }
}
