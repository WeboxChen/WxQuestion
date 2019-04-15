namespace Wei.Core.Domain.Questions
{
    public class QuestionItem : BaseEntity
    {
        /// <summary>
        /// ��Ŀ
        /// </summary>
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// �ı�
        /// </summary>
        public string Text { get; set; }
        public int Sort { get; set; }

    }
}
