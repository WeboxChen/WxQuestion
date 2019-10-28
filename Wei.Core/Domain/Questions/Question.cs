using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wei.Core.Domain.Questions
{
    public class Question: BaseEntity
    {
        private ICollection<QuestionItem> _questionItemList;
        private ICollection<QuestionAnswer> _questionAnswerList;

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
        /// 分组号
        /// </summary>
        public string GroupNo { get; set; }
        /// <summary>
        /// 结果匹配类型
        /// </summary>
        public string MType { get; set; }
        public MatchingType MatchingType
        {
            get
            {
                MatchingType mtype;
                if (Enum.TryParse<MatchingType>(MType, out mtype))
                    return mtype;
                return MatchingType.Single;
            }
            set
            {
                MType = value.ToString();
            }
        }
        /// <summary>
        /// 问题文本
        /// </summary>
        public string QuestionText
        {
            get
            {
                string temp = "";
                switch (this.AnswerType)
                {
                    case AnswerType.SingleCheck:
                    case AnswerType.MultiCheck:
                        StringBuilder text = new StringBuilder();
                        if (!string.IsNullOrEmpty(Text))
                            text.AppendLine(Text);
                        var questionitemlist = this.QuestionItemList.OrderBy(x => x.Code);
                        foreach (var item in QuestionItemList)
                        {
                            text.AppendFormat("{2}{0}. {1}", item.Code, item.Text, "\n");
                        }
                        temp = text.ToString();
                        break;
                    case AnswerType.Tips:
                        temp = string.Format("{0} 当前第{1}题，共{2}题【继续/终止】", Text, this.Sort.ToDouble(), this.QuestionBank.QuestionList.Max(x => x.Sort).ToDouble());
                        break;
                    case AnswerType.Break:
                        temp = string.Format("{0} 答题已终止，感谢您的参与！", Text);
                        break;
                    default:
                        temp = Text ?? "";
                        break;
                }
                return temp;
            }
        }

        /// <summary>
        /// 选择题选项
        /// </summary>
        public virtual ICollection<QuestionItem> QuestionItemList
        {
            get { return _questionItemList ?? (_questionItemList = new List<QuestionItem>()); }
            set { _questionItemList = value; }
        }

        /// <summary>
        /// 问题答案
        /// </summary>
        public virtual ICollection<QuestionAnswer> QuestionAnswerList
        {
            get { return _questionAnswerList ?? (_questionAnswerList = new List<QuestionAnswer>()); }
            set { _questionAnswerList = value; }
        }
    }
}
