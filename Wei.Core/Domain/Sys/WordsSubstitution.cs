using Wei.Core.Domain.Questions;

namespace Wei.Core.Domain.Sys
{
    public class WordsSubstitution : BaseEntity
    {
        /// <summary>
        /// 题卷id
        /// </summary>
        public int? QuestionBankId { get; set; }
        public virtual QuestionBank QuestionBank { get; set; }

        /// <summary>
        /// 匹配词语
        /// </summary>
        public string Words1 { get; set; }

        /// <summary>
        /// 替换词语
        /// </summary>
        public string Words2 { get; set; }

    }
}
